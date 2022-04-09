import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { MatTableDataSource } from '@angular/material/table';
import { ToDoTask } from '../models/api-models/ToDoTask.model';
import { ToDoTaskView } from '../models/ui-models/taskView.model';
import { TaskService } from './task.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css']
})

export class TasksComponent implements OnInit {

  tasks: ToDoTask[] = []
  taskViews: ToDoTaskView[] = [];

  displayedColumns: string[] = ['taskID', 'taskName', 'dueDate', 'completedDate', 'status', 'assignedTo', 'edit', 'delete'];
  dataSource: MatTableDataSource<ToDoTaskView> = new MatTableDataSource<ToDoTaskView>();
 
  constructor(private taskService: TaskService, private snackBar: MatSnackBar) {

  }

  ngOnInit(): void {
    this.readData();
  }

  onDelete(taskID: number) {
    this.taskService.deleteTask(taskID)
      .subscribe(
        (data: ToDoTask) => {
          this.dataSource.data.splice(0,this.dataSource.data.length);
         this.readData();
          this.snackBar.open('Task details have been deleted', undefined, {
            duration: 2000
          });
        },
        (error) => {
          console.log(error);
        });
  }

  readData(): void {
    this.taskService.getAllTasks()
      .subscribe(
        (data: ToDoTask[]) => {
          this.tasks = data;
          this.tasks.forEach((element) => {
            let viewTask: ToDoTaskView = {
              "taskID": element.taskID, "taskName": element.taskName
              , "dueDate": element.dueDate
              , "completedDate": (element.completedDate != null ? element.completedDate : undefined)
              , "status": element.status
              , "assignedTo": (element.assignedTo != null ? element.assignedTo.firstName + ' ' + element.assignedTo.lastName : "")
            };
            this.taskViews.push(viewTask);
          });

          this.dataSource = new MatTableDataSource<ToDoTaskView>(this.taskViews);
        });
  }
}
