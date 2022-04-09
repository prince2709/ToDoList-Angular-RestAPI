import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { KeyValuePair } from '../models/ui-models/KeyValuePair.model';
import { ToDoTask } from '../models/api-models/ToDoTask.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private baseUrl = 'https://localhost:44336/api/ToDoTask';

  constructor(private httpClient: HttpClient) { }

  getAllTasks(): Observable<ToDoTask[]> {
    return this.httpClient.get<ToDoTask[]>(this.baseUrl);
  }

  getTaskByTaskID(taskID: number): Observable<ToDoTask> {
    return this.httpClient.get<ToDoTask>(this.baseUrl + '/' + taskID);
  }

  getAllStatus(): Observable<KeyValuePair[]> {
    return this.httpClient.get<KeyValuePair[]>(this.baseUrl + '/GetAllStatus');
  }

  addTask(task: ToDoTask): Observable<ToDoTask> {
    return this.httpClient.post<ToDoTask>(this.baseUrl, task);
  }

  updateTask(taskID: number, task: ToDoTask): Observable<ToDoTask> {
    return this.httpClient.put<ToDoTask>(this.baseUrl + '/' + taskID, task);
  }

  deleteTask(taskID: number): Observable<ToDoTask> {
    return this.httpClient.delete<ToDoTask>(this.baseUrl + '/' + taskID);
  }
}
