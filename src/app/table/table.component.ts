import { CommonModule } from '@angular/common';
import { get } from '@aws-amplify/api';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';

interface Element {
  name: string;
}

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
  imports: [MatButtonModule, MatTableModule, CommonModule],
  standalone: true
})
export class TableComponent {
  displayedColumns: string[] = ['select', 'name'];
  dataSource: Element[] = [];
  existingElements: string[] = [];
  selectedElement: Element | null = null;
  response: any;
  async fetchData() {
    const apiName = 'tuApiNombre'; // Nombre del API configurado en Amplify
    const path = '/ruta-de-tu-funcion'; // Ruta para acceder a la función Lambda

    try {
      const restOperation = get({
        apiName: 'fondosAPI',
        path: '/fondos',

      });
      const { body } = await restOperation.response;
      this.response = await body.json();
      this.dataSource = this.response;
    } catch (error) {
      console.error('Error al hacer la solicitud GET:', error);
    }
  }

  ngOnInit() {


    this.fetchData();
  }

  onSelect(element: Element) {
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

  addBackground() {
    console.log('Añadiendo fondo al elemento:', this.selectedElement);
    // Aquí puedes agregar la lógica para añadir el fondo al elemento seleccionado
  }

  removeBackground() {
    console.log('Quitando fondo del elemento:', this.selectedElement);
    // Aquí puedes agregar la lógica para quitar el fondo del elemento seleccionado
  }

  isExisting(element: Element): boolean {
    return this.existingElements.includes(element.name);
  }
}
