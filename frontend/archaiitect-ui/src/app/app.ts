import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class AppComponent {

  requirement = '';
  architectureResult: any = null;
  isLoading = false;

  constructor(private http: HttpClient) {}

  generateArchitecture() {

    if (!this.requirement.trim()) {
      alert('Enter requirement');
      return;
    }

    this.isLoading = true;

    this.http.post<any>(
      'http://localhost:5280/api/architecture/generate',
      {
        requirement: this.requirement
      }
    )
    .subscribe({
      next: (res) => {

        console.log(res);

        this.architectureResult = res;

        this.isLoading = false;
      },

      error: (err) => {

        console.log(err);

        alert('API Error');

        this.isLoading = false;
      }
    });
  }
}