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
  title = 'fondoproject';
  dataClient: any;
  async fetchData() {
    const apiName = 'tuApiNombre'; // Nombre del API configurado en Amplify
    const path = '/ruta-de-tu-funcion'; // Ruta para acceder a la función Lambda

    try {
      const restOperation = get({
        apiName: 'fondosAPI',
        path: '/clients/12345',

      });
      const { body } = await restOperation.response;
      const response = await body.json();
      this.dataClient = response
    } catch (error) {
      console.error('Error al hacer la solicitud GET:', error);
    }
  }

  ngOnInit() {


    this.fetchData();
  }
}
