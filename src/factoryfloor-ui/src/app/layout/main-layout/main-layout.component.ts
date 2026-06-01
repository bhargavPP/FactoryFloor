import { Component } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';

import { MaterialModule } from '../../shared/material/material.module';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,

    MaterialModule
  ],
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss']
})
export class MainLayoutComponent {

}
