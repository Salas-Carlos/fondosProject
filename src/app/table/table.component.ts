import { CommonModule } from '@angular/common';
import { get, post } from '@aws-amplify/api';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';

interface Fondo {
  name: string;
  amount: number;
  id: string;
}

interface Client {
  amountClient: number;
  id: string;
  funds: string[];
}
interface Transaction {

  amount: number;

}



@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
  imports: [MatButtonModule, MatTableModule, CommonModule],
  standalone: true
})
export class TableComponent {

  @Input() dataClient: Client = { amountClient: 0, id: '', funds: [] };

  @Output() dataClientChange = new EventEmitter<Client>();

  displayedColumns: string[] = ['select', 'name', 'amount'];
  dataSource: Fondo[] = [];

  selectedElement: Fondo = { name: '', amount: 0, id: '' };

  async fetchData() {
    try {
      const restOperation = get({
        apiName: 'fondosAPI',
        path: '/fondos',

      });
      const { body } = await restOperation.response;
      this.dataSource = await body.json() as unknown as Fondo[];

    } catch (error) {
      console.error('Error al hacer la solicitud GET:', error);
    }
  }

  ngOnInit() {
    this.fetchData();
  }

  onSelect(element: Fondo) {
    this.selectedElement = element;
  }

  getButtonLabel(): string {
    if (this.selectedElement) {
      return this.isExisting(this.selectedElement) ? 'Quitar fondo' : 'Añadir fondo';
    }
    return 'Seleccionar elemento';
  }

  handleButtonClick() {
    if (this.selectedElement) {
      if (this.isExisting(this.selectedElement)) {
        this.removeBackground();
      } else {
        this.addBackground();
      }
    }
  }

  async addBackground() {

    if (this.dataClient.amountClient >= this.selectedElement.amount) {
      const restOperation = post({
        apiName: 'fondosAPI', path: '/transactions', options: {
          body: {
            PK: 'TRANSACTION',
            amount: -1 * this.selectedElement.amount,
            clientId: this.dataClient.id,
            fondoId: this.selectedElement.id
          }
        }
      })

      const { body, statusCode } = await restOperation.response;
      if (statusCode === 201) {
        this.dataClient.funds.push(this.selectedElement.id);
        const response = await body.json() as unknown as Transaction;
        this.dataClient.amountClient += response.amount;
        this.dataClientChange.emit(this.dataClient);
      }

    }
  }

  async removeBackground() {

    const restOperation = post({
      apiName: 'fondosAPI', path: '/transactions', options: {
        body: {
          PK: 'TRANSACTION',
          amount: this.selectedElement.amount,
          clientId: this.dataClient.id,
          fondoId: this.selectedElement.id
        }
      }
    })
    const { body, statusCode } = await restOperation.response;
    if (statusCode === 201) {
      this.dataClient.funds = this.dataClient.funds.filter((name) => name !== this.selectedElement.id);
      const response = await body.json() as unknown as Transaction;
      this.dataClient.amountClient += response.amount;
      this.dataClientChange.emit(this.dataClient);
    }
  }

  isExisting(element: Fondo): boolean {
    return this.dataClient.funds.includes(element.id);
  }
}
