import { get } from '@aws-amplify/api';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'fondoproject';

  async fetchData() {
    const apiName = 'tuApiNombre'; // Nombre del API configurado en Amplify
    const path = '/ruta-de-tu-funcion'; // Ruta para acceder a la función Lambda

    try {
      const restOperation = get({
        apiName: 'fondosAPI',
        path: '/clients',

      });
      const { body } = await restOperation.response;
      const response = await body.json();

      console.log('POST call succeeded');
      console.log(response);
    } catch (error) {
      console.error('Error al hacer la solicitud GET:', error);
    }
  }

  ngOnInit() {


    this.fetchData();
  }
}
