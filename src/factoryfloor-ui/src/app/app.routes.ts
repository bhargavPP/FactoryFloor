import { Routes } from '@angular/router';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { MachinesComponent } from './features/machines/machines.component';
import { TelemetryComponent } from './features/telemetry/telemetry.component';
import { AlertsComponent } from './features/alerts/alerts.component';
import { LoginComponent } from './features/login/login.component';

export const routes: Routes = [{
  path: '',
  component: MainLayoutComponent,
  children: [
    { path: '', component: DashboardComponent },
    { path: 'machines', component: MachinesComponent },
    { path: 'telemetry', component: TelemetryComponent },
    { path: 'alerts', component: AlertsComponent },
    {
      path: 'login',
      component: LoginComponent
    },
  ]
}
];
