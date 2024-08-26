import { get } from '@aws-amplify/api';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TableComponent } from './table/table.component';



@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, TableComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  dataClient: any;
  async fetchData() {
    try {
      const restOperation = get({
        apiName: 'fondosAPI',
        path: '/clients/12345',

      });
      const { body } = await restOperation.response;
      this.dataClient = await body.json();

    } catch (error) {
      console.error('Error al hacer la solicitud GET:', error);
    }
  }
  onDataClientChange(updatedClient: any) {
    // Actualiza la propiedad client con los nuevos valores
    this.dataClient = updatedClient;
  }
  ngOnInit() {
    this.fetchData();
  }
}
