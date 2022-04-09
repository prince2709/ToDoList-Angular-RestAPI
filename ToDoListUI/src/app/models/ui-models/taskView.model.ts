
export interface ToDoTaskView {
    taskID: number,
    taskName: string,
    dueDate?: Date,
    completedDate?: Date,
    status: string,
    assignedTo: string
}