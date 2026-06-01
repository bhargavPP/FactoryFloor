import { Routes } from '@angular/router';
import { DashboardComponent } from './layout/features/dashboard/dashboard.component';
import { MachinesComponent } from './layout/features/machines/machines.component';
import { TelemetryComponent } from './layout/features/telemetry/telemetry.component';
import { AlertsComponent } from './layout/features/alerts/alerts.component';

export const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'machines', component: MachinesComponent },
  { path: 'telemetry', component: TelemetryComponent },
  { path: 'alerts', component: AlertsComponent }
];
