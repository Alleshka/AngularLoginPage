import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegistrationWizardComponent } from './components/registration-wizard/registration-wizard.component';

const routes: Routes = [
  { path: 'register', component: RegistrationWizardComponent },
  { path: '**', redirectTo: "register" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
