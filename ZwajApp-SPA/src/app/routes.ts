import { Routes } from '@angular/router'
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { ListResolver } from './_resolvers/lists.resolver';
export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always'
        , canActivate: [AuthGuard],
        children: [
            { path: 'members', component: MemberListComponent,resolve:{
                users:MemberListResolver
            } },
            { path: 'member/edit', component: MemberEditComponent,resolve:{
                user:MemberEditResolver },canDeactivate:[PreventUnsavedChangesGuard]
             },
            { path: 'members/:id', component: MemberDetailComponent,resolve:{
                user:MemberDetailResolver
            } },
            { path: 'lists', component: ListsComponent ,resolve:{
                users : ListResolver
            }},
            { path: 'messages', component: MessagesComponent }
        ]
    },
   
    { path: '**', redirectTo: '', pathMatch: 'full' }
];