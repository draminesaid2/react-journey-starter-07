import React, { useState, useEffect } from 'react';
import { Calendar } from 'react-day-picker';
import { cn } from "@/lib/utils";

interface Task {
  id: string;
  text: string;
  completed: boolean;
  date: string;
  createdAt: string;
}

interface Reminder {
  id: string;
  text: string;
  time: string;
}

interface TodoAppProps {
  onClose: () => void;
}

const TodoApp = ({ onClose }: TodoAppProps) => {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [newTask, setNewTask] = useState('');
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [activeTab, setActiveTab] = useState<'tasks' | 'calendar' | 'reminders'>('tasks');
  const [reminders, setReminders] = useState<Reminder[]>([]);
  const [newReminder, setNewReminder] = useState('');
  const [reminderTime, setReminderTime] = useState('');

  useEffect(() => {
    const storedTasks = localStorage.getItem('todoTasks');
    if (storedTasks) {
      setTasks(JSON.parse(storedTasks));
    }
  }, []);

  useEffect(() => {
    const storedReminders = localStorage.getItem('todoReminders');
    if (storedReminders) {
      setReminders(JSON.parse(storedReminders));
    }
  }, []);

  const handleDateSelect = (date: Date | undefined) => {
    if (date) {
      setSelectedDate(date);
      setActiveTab('calendar');
    }
  };

  const addTask = () => {
    if (newTask.trim()) {
      const task: Task = {
        id: Date.now().toString(),
        text: newTask,
        completed: false,
        date: selectedDate.toISOString().split('T')[0],
        createdAt: new Date().toISOString()
      };
      const updatedTasks = [...tasks, task];
      setTasks(updatedTasks);
      localStorage.setItem('todoTasks', JSON.stringify(updatedTasks));
      setNewTask('');
    }
  };

  const toggleTask = (id: string) => {
    const updatedTasks = tasks.map((task) =>
      task.id === id ? { ...task, completed: !task.completed } : task
    );
    setTasks(updatedTasks);
    localStorage.setItem('todoTasks', JSON.stringify(updatedTasks));
  };

  const deleteTask = (id: string) => {
    const updatedTasks = tasks.filter((task) => task.id !== id);
    setTasks(updatedTasks);
    localStorage.setItem('todoTasks', JSON.stringify(updatedTasks));
  };

  const addReminder = () => {
    if (newReminder.trim() && reminderTime) {
      const reminder: Reminder = {
        id: Date.now().toString(),
        text: newReminder,
        time: reminderTime,
      };
      const updatedReminders = [...reminders, reminder];
      setReminders(updatedReminders);
      localStorage.setItem('todoReminders', JSON.stringify(updatedReminders));
      setNewReminder('');
      setReminderTime('');
    }
  };

  const deleteReminder = (id: string) => {
    const updatedReminders = reminders.filter((reminder) => reminder.id !== id);
    setReminders(updatedReminders);
    localStorage.setItem('todoReminders', JSON.stringify(updatedReminders));
  };

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
      <div className="bg-background/95 backdrop-blur-md border border-cyan-500/30 rounded-xl w-full max-w-4xl h-[80vh] shadow-2xl">
        {/* Header */}
        <div className="flex items-center justify-between p-4 border-b border-cyan-500/20">
          <h2 className="text-xl font-bold text-cyan-400">Future Todo & Calendar</h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-white transition-colors"
          >
            ✕
          </button>
        </div>

        {/* Tabs */}
        <div className="flex border-b border-cyan-500/20">
          {['tasks', 'calendar', 'reminders'].map((tab) => (
            <button
              key={tab}
              onClick={() => setActiveTab(tab as any)}
              className={`px-6 py-3 capitalize font-medium transition-colors ${
                activeTab === tab
                  ? 'text-cyan-400 border-b-2 border-cyan-400'
                  : 'text-gray-400 hover:text-white'
              }`}
            >
              {tab}
            </button>
          ))}
        </div>

        {/* Content */}
        <div className="p-4 h-[calc(100%-8rem)] overflow-auto">
          {activeTab === 'tasks' && (
            <div className="space-y-4">
              {/* Add Task */}
              <div className="flex gap-2">
                <input
                  type="text"
                  value={newTask}
                  onChange={(e) => setNewTask(e.target.value)}
                  onKeyPress={(e) => e.key === 'Enter' && addTask()}
                  placeholder="Add a new task..."
                  className="flex-1 px-3 py-2 bg-background/50 border border-cyan-500/30 rounded-lg text-white placeholder-gray-400 focus:outline-none focus:border-cyan-400"
                />
                <button
                  onClick={addTask}
                  className="px-4 py-2 bg-cyan-500 text-black rounded-lg hover:bg-cyan-400 transition-colors font-medium"
                >
                  Add
                </button>
              </div>

              {/* Tasks List */}
              <div className="space-y-2">
                {tasks
                  .filter(task => task.date === selectedDate.toISOString().split('T')[0])
                  .map((task) => (
                    <div
                      key={task.id}
                      className={`flex items-center gap-3 p-3 rounded-lg border ${
                        task.completed
                          ? 'bg-green-500/10 border-green-500/30'
                          : 'bg-background/50 border-cyan-500/30'
                      }`}
                    >
                      <input
                        type="checkbox"
                        checked={task.completed}
                        onChange={() => toggleTask(task.id)}
                        className="w-4 h-4"
                      />
                      <span
                        className={`flex-1 ${
                          task.completed ? 'line-through text-gray-400' : 'text-white'
                        }`}
                      >
                        {task.text}
                      </span>
                      <button
                        onClick={() => deleteTask(task.id)}
                        className="text-red-400 hover:text-red-300 transition-colors"
                      >
                        Delete
                      </button>
                    </div>
                  ))}
              </div>
            </div>
          )}

          {activeTab === 'calendar' && (
            <div className="flex gap-6">
              <div className="flex-1">
                <Calendar
                  mode="single"
                  selected={selectedDate}
                  onSelect={handleDateSelect}
                  className="rounded-md border border-cyan-500/30"
                  modifiers={{
                    hasTasks: tasks.map(task => new Date(task.date))
                  }}
                  modifiersStyles={{
                    hasTasks: { backgroundColor: 'rgba(6, 182, 212, 0.3)' }
                  }}
                />
              </div>
              <div className="flex-1">
                <h3 className="text-lg font-semibold text-cyan-400 mb-4">
                  Tasks for {selectedDate.toLocaleDateString()}
                </h3>
                <div className="space-y-2">
                  {tasks
                    .filter(task => task.date === selectedDate.toISOString().split('T')[0])
                    .map((task) => (
                      <div
                        key={task.id}
                        className={`p-3 rounded-lg border ${
                          task.completed
                            ? 'bg-green-500/10 border-green-500/30'
                            : 'bg-background/50 border-cyan-500/30'
                        }`}
                      >
                        <span className={task.completed ? 'line-through text-gray-400' : 'text-white'}>
                          {task.text}
                        </span>
                      </div>
                    ))}
                </div>
              </div>
            </div>
          )}

          {activeTab === 'reminders' && (
            <div className="space-y-4">
              {/* Add Reminder */}
              <div className="flex gap-2">
                <input
                  type="text"
                  value={newReminder}
                  onChange={(e) => setNewReminder(e.target.value)}
                  onKeyPress={(e) => e.key === 'Enter' && addReminder()}
                  placeholder="Add a reminder..."
                  className="flex-1 px-3 py-2 bg-background/50 border border-cyan-500/30 rounded-lg text-white placeholder-gray-400 focus:outline-none focus:border-cyan-400"
                />
                <input
                  type="datetime-local"
                  value={reminderTime}
                  onChange={(e) => setReminderTime(e.target.value)}
                  className="px-3 py-2 bg-background/50 border border-cyan-500/30 rounded-lg text-white focus:outline-none focus:border-cyan-400"
                />
                <button
                  onClick={addReminder}
                  className="px-4 py-2 bg-cyan-500 text-black rounded-lg hover:bg-cyan-400 transition-colors font-medium"
                >
                  Add
                </button>
              </div>

              {/* Reminders List */}
              <div className="space-y-2">
                {reminders.map((reminder) => (
                  <div
                    key={reminder.id}
                    className="flex items-center justify-between p-3 rounded-lg border bg-background/50 border-cyan-500/30"
                  >
                    <div>
                      <p className="text-white">{reminder.text}</p>
                      <p className="text-sm text-gray-400">
                        {new Date(reminder.time).toLocaleString()}
                      </p>
                    </div>
                    <button
                      onClick={() => deleteReminder(reminder.id)}
                      className="text-red-400 hover:text-red-300 transition-colors"
                    >
                      Delete
                    </button>
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default TodoApp;
