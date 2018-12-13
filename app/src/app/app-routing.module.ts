import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent, AtanetComponent } from './components';
import { UserProfileComponent } from './components/user-profile/user-profile.component';

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
        path: 'user',
        component: UserProfileComponent
    },
    {
        path: '**',
        redirectTo: 'login'
    }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
