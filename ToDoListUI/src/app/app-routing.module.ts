import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TasksComponent } from './tasks/tasks.component';
import { UpdateTaskComponent } from './tasks/update-task/update-task.component';

const routes: Routes = [
  {
    path: '',
    component: TasksComponent
  },
  {
    path: 'tasks',
    component: TasksComponent
  }
  ,
  {
    path: 'updateTask/:taskID',
    component: UpdateTaskComponent
  },
  {
    path: 'addTask',
    component: UpdateTaskComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
