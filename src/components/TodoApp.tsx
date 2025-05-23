
import React, { useState, useEffect } from 'react';
import { X, Calendar as CalendarIcon, Check, Plus, Clock } from 'lucide-react';
import { cn } from '@/lib/utils';
import { Calendar } from '@/components/ui/calendar';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { format } from 'date-fns';

interface Todo {
  id: string;
  text: string;
  completed: boolean;
  dueDate?: Date;
}

interface Reminder {
  id: string;
  text: string;
  date: Date;
}

interface TodoAppProps {
  onClose: () => void;
}

const TodoApp = ({ onClose }: TodoAppProps) => {
  // State for todos
  const [todos, setTodos] = useState<Todo[]>(() => {
    const savedTodos = localStorage.getItem('todos');
    return savedTodos ? JSON.parse(savedTodos) : [];
  });
  
  // State for reminders
  const [reminders, setReminders] = useState<Reminder[]>(() => {
    const savedReminders = localStorage.getItem('reminders');
    return savedReminders ? JSON.parse(savedReminders) : [];
  });
  
  // Form inputs
  const [newTodo, setNewTodo] = useState('');
  const [newReminder, setNewReminder] = useState('');
  const [selectedDate, setSelectedDate] = useState<Date | undefined>(new Date());
  const [activeTab, setActiveTab] = useState('todos');

  // Save todos and reminders to localStorage when they change
  useEffect(() => {
    localStorage.setItem('todos', JSON.stringify(todos));
  }, [todos]);
  
  useEffect(() => {
    localStorage.setItem('reminders', JSON.stringify(reminders));
  }, [reminders]);

  // Handle adding a new todo
  const handleAddTodo = () => {
    if (newTodo.trim()) {
      const newTodoItem: Todo = {
        id: Date.now().toString(),
        text: newTodo.trim(),
        completed: false,
        dueDate: selectedDate
      };
      setTodos([...todos, newTodoItem]);
      setNewTodo('');
    }
  };

  // Handle adding a new reminder
  const handleAddReminder = () => {
    if (newReminder.trim() && selectedDate) {
      const newReminderItem: Reminder = {
        id: Date.now().toString(),
        text: newReminder.trim(),
        date: selectedDate
      };
      setReminders([...reminders, newReminderItem]);
      setNewReminder('');
    }
  };

  // Toggle todo completion
  const toggleTodoCompleted = (id: string) => {
    setTodos(
      todos.map((todo) => 
        todo.id === id ? { ...todo, completed: !todo.completed } : todo
      )
    );
  };

  // Delete a todo
  const deleteTodo = (id: string) => {
    setTodos(todos.filter((todo) => todo.id !== id));
  };

  // Delete a reminder
  const deleteReminder = (id: string) => {
    setReminders(reminders.filter((reminder) => reminder.id !== id));
  };

  // Get todos for a specific date
  const getTodosForDate = (date: Date) => {
    return todos.filter(todo => 
      todo.dueDate && 
      new Date(todo.dueDate).toDateString() === date.toDateString()
    );
  };

  // Get reminders for a specific date
  const getRemindersForDate = (date: Date) => {
    return reminders.filter(reminder => 
      new Date(reminder.date).toDateString() === date.toDateString()
    );
  };

  // Count items per date for the calendar
  const getDayContent = (date: Date) => {
    const todosCount = getTodosForDate(date).length;
    const remindersCount = getRemindersForDate(date).length;
    const totalCount = todosCount + remindersCount;
    
    if (totalCount === 0) return null;
    
    return (
      <div className="absolute bottom-0 right-0 flex h-3 w-3 items-center justify-center">
        <div className="h-2 w-2 rounded-full bg-green-500"></div>
      </div>
    );
  };

  return (
    <div className="flex flex-col w-full h-full bg-background border rounded-lg shadow-lg backdrop-blur-sm overflow-hidden">
      {/* Header */}
      <div className="flex items-center justify-between bg-gradient-to-r from-purple-600 via-blue-500 to-indigo-600 p-3">
        <h2 className="text-lg font-semibold text-white">Futuristic Planner</h2>
        <button onClick={onClose} className="text-white hover:text-gray-200">
          <X size={20} />
        </button>
      </div>

      {/* Main content */}
      <Tabs defaultValue="todos" className="flex flex-col h-full" onValueChange={(value) => setActiveTab(value)}>
        <TabsList className="w-full justify-start p-1 bg-muted/50">
          <TabsTrigger value="todos" className="rounded-md">Tasks</TabsTrigger>
          <TabsTrigger value="calendar" className="rounded-md">Calendar</TabsTrigger>
          <TabsTrigger value="reminders" className="rounded-md">Reminders</TabsTrigger>
        </TabsList>

        {/* Tasks tab */}
        <TabsContent value="todos" className="flex-1 overflow-hidden flex flex-col">
          <div className="p-4 flex items-center space-x-2">
            <Input 
              value={newTodo}
              onChange={(e) => setNewTodo(e.target.value)}
              placeholder="Add a new task..."
              onKeyPress={(e) => e.key === 'Enter' && handleAddTodo()}
              className="flex-1 bg-background/80 backdrop-blur-sm"
            />
            <Popover>
              <PopoverTrigger asChild>
                <Button variant="outline" size="icon" className="bg-background/80">
                  <CalendarIcon size={16} />
                </Button>
              </PopoverTrigger>
              <PopoverContent className="w-auto p-0" align="start">
                <Calendar
                  mode="single"
                  selected={selectedDate}
                  onSelect={setSelectedDate}
                  initialFocus
                  className="p-3 pointer-events-auto"
                />
              </PopoverContent>
            </Popover>
            <Button onClick={handleAddTodo} className="bg-gradient-to-r from-purple-600 to-blue-600 hover:from-purple-700 hover:to-blue-700">
              <Plus size={16} />
            </Button>
          </div>

          <div className="flex-1 overflow-y-auto p-3 space-y-2">
            {todos.length === 0 ? (
              <div className="text-center py-8 text-muted-foreground">
                No tasks yet. Add one above!
              </div>
            ) : (
              todos.map((todo) => (
                <div 
                  key={todo.id}
                  className={cn(
                    "flex items-center justify-between p-3 rounded-lg transition-all",
                    todo.completed 
                      ? "bg-muted/40 text-muted-foreground" 
                      : "bg-card hover:bg-accent/10"
                  )}
                >
                  <div className="flex items-center space-x-3 flex-1">
                    <button 
                      onClick={() => toggleTodoCompleted(todo.id)}
                      className={cn(
                        "h-5 w-5 rounded-full flex items-center justify-center border transition-colors",
                        todo.completed 
                          ? "bg-green-500 border-green-600" 
                          : "border-gray-400"
                      )}
                    >
                      {todo.completed && <Check size={12} className="text-white" />}
                    </button>
                    <span className={cn(todo.completed && "line-through")}>
                      {todo.text}
                    </span>
                  </div>
                  
                  <div className="flex items-center space-x-2">
                    {todo.dueDate && (
                      <span className="text-xs text-muted-foreground">
                        {format(new Date(todo.dueDate), 'MMM d')}
                      </span>
                    )}
                    <button 
                      onClick={() => deleteTodo(todo.id)}
                      className="text-muted-foreground hover:text-destructive"
                    >
                      <X size={16} />
                    </button>
                  </div>
                </div>
              ))
            )}
          </div>
        </TabsContent>

        {/* Calendar tab */}
        <TabsContent value="calendar" className="flex-1 overflow-y-auto p-4">
          <div className="bg-card rounded-lg p-4 shadow-sm">
            <Calendar
              mode="single"
              selected={selectedDate}
              onSelect={setSelectedDate}
              className="mx-auto pointer-events-auto"
              components={{
                DayContent: ({ date, ...props }) => (
                  <div className="relative w-full h-full flex items-center justify-center">
                    {props.children}
                    {getDayContent(date)}
                  </div>
                ),
              }}
            />
          </div>

          {selectedDate && (
            <div className="mt-6">
              <h3 className="text-sm font-medium mb-2 flex items-center">
                <CalendarIcon size={16} className="mr-2" /> 
                {format(selectedDate, 'MMMM d, yyyy')}
              </h3>
              
              <div className="space-y-4">
                <div>
                  <h4 className="text-xs uppercase text-muted-foreground font-medium tracking-wide mb-2">Tasks</h4>
                  {getTodosForDate(selectedDate).length === 0 ? (
                    <p className="text-sm text-muted-foreground">No tasks for this day</p>
                  ) : (
                    <div className="space-y-1">
                      {getTodosForDate(selectedDate).map(todo => (
                        <div 
                          key={todo.id}
                          className={cn(
                            "flex items-center space-x-2 p-2 rounded-md",
                            todo.completed ? "text-muted-foreground" : ""
                          )}
                        >
                          <button 
                            onClick={() => toggleTodoCompleted(todo.id)}
                            className={cn(
                              "h-4 w-4 rounded-full flex items-center justify-center border transition-colors",
                              todo.completed 
                                ? "bg-green-500 border-green-600" 
                                : "border-gray-400"
                            )}
                          >
                            {todo.completed && <Check size={10} className="text-white" />}
                          </button>
                          <span className={cn("text-sm", todo.completed && "line-through")}>
                            {todo.text}
                          </span>
                        </div>
                      ))}
                    </div>
                  )}
                </div>

                <div>
                  <h4 className="text-xs uppercase text-muted-foreground font-medium tracking-wide mb-2">Reminders</h4>
                  {getRemindersForDate(selectedDate).length === 0 ? (
                    <p className="text-sm text-muted-foreground">No reminders for this day</p>
                  ) : (
                    <div className="space-y-1">
                      {getRemindersForDate(selectedDate).map(reminder => (
                        <div key={reminder.id} className="flex items-center justify-between p-2 rounded-md bg-accent/10">
                          <div className="flex items-center space-x-2">
                            <Clock size={14} />
                            <span className="text-sm">{reminder.text}</span>
                          </div>
                          <button 
                            onClick={() => deleteReminder(reminder.id)}
                            className="text-muted-foreground hover:text-destructive"
                          >
                            <X size={14} />
                          </button>
                        </div>
                      ))}
                    </div>
                  )}
                </div>
              </div>
            </div>
          )}
        </TabsContent>

        {/* Reminders tab */}
        <TabsContent value="reminders" className="flex-1 overflow-hidden flex flex-col">
          <div className="p-4 flex items-center space-x-2">
            <Input 
              value={newReminder}
              onChange={(e) => setNewReminder(e.target.value)}
              placeholder="Add a new reminder..."
              onKeyPress={(e) => e.key === 'Enter' && handleAddReminder()}
              className="flex-1 bg-background/80 backdrop-blur-sm"
            />
            <Popover>
              <PopoverTrigger asChild>
                <Button variant="outline" size="icon" className="bg-background/80">
                  <CalendarIcon size={16} />
                </Button>
              </PopoverTrigger>
              <PopoverContent className="w-auto p-0" align="start">
                <Calendar
                  mode="single"
                  selected={selectedDate}
                  onSelect={setSelectedDate}
                  initialFocus
                  className="p-3 pointer-events-auto"
                />
              </PopoverContent>
            </Popover>
            <Button onClick={handleAddReminder} className="bg-gradient-to-r from-purple-600 to-blue-600 hover:from-purple-700 hover:to-blue-700">
              <Plus size={16} />
            </Button>
          </div>

          <div className="flex-1 overflow-y-auto p-3 space-y-2">
            {reminders.length === 0 ? (
              <div className="text-center py-8 text-muted-foreground">
                No reminders yet. Add one above!
              </div>
            ) : (
              reminders.map((reminder) => (
                <div 
                  key={reminder.id}
                  className="flex items-center justify-between p-3 rounded-lg bg-card hover:bg-accent/10 transition-all"
                >
                  <div className="flex items-center space-x-3">
                    <Clock size={16} className="text-blue-500" />
                    <span>{reminder.text}</span>
                  </div>
                  
                  <div className="flex items-center space-x-3">
                    <span className="text-xs text-muted-foreground">
                      {format(new Date(reminder.date), 'MMM d, yyyy')}
                    </span>
                    <button 
                      onClick={() => deleteReminder(reminder.id)}
                      className="text-muted-foreground hover:text-destructive"
                    >
                      <X size={16} />
                    </button>
                  </div>
                </div>
              ))
            )}
          </div>
        </TabsContent>
      </Tabs>
    </div>
  );
};

export default TodoApp;
