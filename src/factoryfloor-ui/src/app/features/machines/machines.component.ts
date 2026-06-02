import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from '../../shared/material/material.module';

import { Machine } from '../../shared/models/machine.model';
import { MachineService } from '../../services/machine.service';

@Component({
  selector: 'app-machines',
  standalone: true,
  imports: [CommonModule,
    MaterialModule],
  templateUrl: './machines.component.html',
  styleUrl: './machines.component.scss'
})
export class MachinesComponent {
  private machineService = inject(MachineService);

  machines: Machine[] = [];

  ngOnInit(): void {
    this.loadMachines();
  }

  loadMachines(): void {
    this.machineService.getMachines()
      .subscribe({
        next: (response) => {
          this.machines = response;
        },
        error: (error) => {
          console.error(error);
        }
      });
  }
}
