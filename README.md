Here are the design patterns used:

1. **Model-View-Controller (MVC) Pattern**:
    - This is the core architectural pattern used in the ASP.NET MVC framework.
    - **Model**: Represents the data and the business logic. Example: `Note`, `User`, `Tag` classes.
    - **View**: Represents the presentation layer. Example: Razor views like `Index.cshtml`.
    - **Controller**: Handles user input and updates the model and view accordingly. Example: `NotesController`.

2. **Repository Pattern**:
    - Used to abstract the data access logic and provide a clean separation between the data access layer and the business logic.
    - Example: `IRepository<T>` interface and its implementation `Repository<T>`, providing methods like `GetAllAsync`, `GetByIdAsync`, `CreateAsync`, etc.

3. **Dependency Injection (DI)**:
    - Used to inject dependencies into classes rather than having the classes create the dependencies themselves.
    - Example: The services (`INoteService`, `IUserService`) and repositories (`IRepository<T>`) are injected into the controllers and other services through the constructor.

4. **Service Layer Pattern**:
    - Provides an additional layer of abstraction over the repository to encapsulate business logic.
    - Example: `INoteService` and `IUserService` interfaces and their implementations `NoteService` and `UserService`.

5. **Singleton Pattern**:
    - Ensures that a class has only one instance and provides a global point of access to it.
    - In the context of ASP.NET Core, this is often used for services that should have a single instance throughout the application lifecycle.
    - Example: `IMongoClient` instance is registered as a singleton in the `Startup.cs`.

6. **Unit of Work Pattern** (implied):
    - While not explicitly implemented, the repository pattern often implies a unit of work to ensure a series of operations are completed successfully.
    - Example: Operations within `Repository<T>` are implicitly handled as units of work, especially within a transaction context.

### Implementation Details

1. **MVC Pattern**:
    - Controllers handle HTTP requests and return views or data.
    - Models define the structure of data.
    - Views display data to the user.

2. **Repository Pattern**:
    ```csharp
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<T> GetByIdAsync(string id);
        Task CreateAsync(T entity);
        Task UpdateAsync(string id, T entity);
        Task DeleteAsync(string id);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public Repository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);
        }

        // Implementation of IRepository methods
    }
    ```

3. **Dependency Injection**:
    ```csharp
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMongoClient, MongoClient>(sp => 
                new MongoClient(Configuration.GetConnectionString("MongoDb")));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
    ```

4. **Service Layer**:
    ```csharp
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetNotesByUserIdAsync(string userId);
        Task<Note> GetByIdAsync(string id);
        Task CreateAsync(Note note);
        Task UpdateAsync(string id, Note note);
        Task DeleteAsync(string id);
    }

    public class NoteService : INoteService
    {
        private readonly IRepository<Note> _noteRepository;

        public NoteService(IRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        // Implementation of INoteService methods
    }
    ```
