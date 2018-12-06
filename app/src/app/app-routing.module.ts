import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent, AtanetComponent } from './components';

const appRoutes: Routes = [
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: '',
        component: AtanetComponent
    },
    {
        path: '**',
        redirectTo: 'login'
    }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
