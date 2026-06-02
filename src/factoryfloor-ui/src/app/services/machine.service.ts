import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Machine } from '../shared/models/machine.model';

import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MachineService {
  private http=inject(HttpClient);

  private apiUrl = `${environment.apiUrl}/machines`;

  getMachines(): Observable<Machine[]> {
    return this.http.get<Machine[]>(this.apiUrl); 
  }

}
