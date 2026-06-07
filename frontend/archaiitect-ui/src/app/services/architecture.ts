import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ArchitectureService {

  private apiUrl = 'http://localhost:5280/api/Architecture/generate';

  constructor(private http: HttpClient) { }

  generateArchitecture(requirement: string): Observable<any> {
    return this.http.post(this.apiUrl, {
      requirement: requirement
    });
  }
}