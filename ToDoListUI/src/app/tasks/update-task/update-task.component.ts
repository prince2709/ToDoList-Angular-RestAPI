import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { KeyValuePair } from 'src/app/models/ui-models/KeyValuePair.model';
import { ToDoTask } from 'src/app/models/api-models/ToDoTask.model';
import { TaskService } from '../task.service';
import { UserService } from 'src/app/User/user.service';
import { User } from 'src/app/models/api-models/User.model';
import { UserView } from 'src/app/models/ui-models/userView.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-update-task',
  templateUrl: './update-task.component.html',
  styleUrls: ['./update-task.component.css']
})
export class UpdateTaskComponent implements OnInit {
  taskToUpdate: ToDoTask =
    {
      assignedTo: {
        emailAddress: ''
        , firstName: ''
        , lastName: ''
        , userID: 0
      },
      completedDate: undefined
      , status: ''
      , dueDate: undefined
      , taskID: 0
      , taskName: ''
      , assignedToUserID: 0
    };

  taskID: string | null | undefined;
  userList: User[] = [];
  userViewList: UserView[] = [];
  tastStatusList: KeyValuePair[] = [];
  isNewTask: boolean = false;
  header: string = '';
  @ViewChild('taskDetailsForm') taskDetailsForm?: NgForm;

  constructor(private taskService: TaskService, private readonly route: ActivatedRoute
    , private userService: UserService, private snackBar: MatSnackBar, private router: Router) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(
      (params) => {
        this.taskID = params.get('taskID');
        if (this.taskID && Number(this.taskID) != NaN && Number(this.taskID) != 0) {
          this.isNewTask = false;
          this.header = 'Edit Task'
          this.taskService.getTaskByTaskID(Number(this.taskID))
            .subscribe(
              (data: ToDoTask) => {
                this.taskToUpdate = data;
                if (this.taskToUpdate.dueDate) {
                  this.taskToUpdate.dueDate = new Date(this.taskToUpdate.dueDate);
                }
                if (!this.taskToUpdate.assignedToUserID) {
                  this.taskToUpdate.assignedToUserID = 0;
                }
              });
        }
        else {
          this.header = 'Add New Task'
          this.isNewTask = true;
        }

        this.taskService.getAllStatus()
          .subscribe(
            (data: KeyValuePair[]) => {
              this.tastStatusList = data;
            }
          );

        this.userService.getAllUsers()
          .subscribe(
            (data: User[]) => {
              this.userList = data;
              this.userList.forEach((element) => {
                let userView: UserView = {
                  "userID": element.userID
                  , "userName": element.firstName + ' ' + element.lastName
                  , "emailAddress": element.emailAddress
                };
                this.userViewList.push(userView);
              });
            });
      });
  }

  onUpdate(): void {
    if (this.taskDetailsForm?.form.valid) {
      this.taskService.updateTask(this.taskToUpdate.taskID, this.taskToUpdate)
        .subscribe(
          (data: ToDoTask) => {
            this.snackBar.open('Task details have been updated', undefined, {
              duration: 2000
            });
          },
          (error) => {
            console.log(error);
          });
    }
  }

  onAdd(): void {
    if (this.taskDetailsForm?.form.valid) {
      this.taskService.addTask(this.taskToUpdate)
        .subscribe(
          (data: ToDoTask) => {
            this.snackBar.open('Task details have been added', undefined, {
              duration: 2000
            });
            setTimeout(() => {
              this.router.navigateByUrl('/tasks');
            }, 2000);
          },
          (error) => {
            console.log(error);
          });
    }
  }

}
