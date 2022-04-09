import { User } from "./User.model";

export interface ToDoTask {
    taskID: number,
    taskName: string,
    dueDate?: Date,
    completedDate?: Date,
    status: string,
    assignedToUserID? : number
    assignedTo: User
}