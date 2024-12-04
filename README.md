# Learning Management System (LMS)

## 🚀 Technologies, Packages & Patterns

### Backend Technologies
- **ASP.NET Core API**: 🖥️ For building robust, scalable, and high-performance APIs.
- **C#**: 🔧 The primary programming language for backend logic.
- **EF Core**: 📊 For object-relational mapping (ORM) and database operations.
- **LINQ**: 🔍 For querying collections and databases with clean and concise syntax.

### 🔐 Security
- **JWT (JSON Web Tokens)**: 🔑 For secure authentication and stateless authorization.
- **Identity**: 👤 For managing users, roles, and authentication workflows.
- **Role-Based Authorization**: 🚪 To restrict access to specific resources or actions based on assigned roles.

### 📐 Patterns
- **CQRS** (Command Query Responsibility Segregation) with MediatR 🌐.
- **Dependency Injection**: 🧩 For managing service lifetimes and enhancing testability.
- **Options Pattern**: ⚙️ For binding and managing configuration settings.
- **Result Pattern**: ✅ For consistent error handling and operation result representation.
- **Unit of Work & Generic Repository Patterns**: 🛠️ To manage data transactions and abstract data access logic.
- **Clean Architecture**: 🏗️ For organizing code into independent layers (e.g., domain, application, infrastructure, and presentation) to ensure maintainability and scalability.

### 🌟 Key Features
- **Pagination and Filtering**: 📄 For efficient data retrieval and user-friendly data presentation.
- **Rate Limiting**: ⏱️ To control API usage and prevent abuse.
- **Exception Handling**: ⚠️ For centralized error processing and improving user feedback.
- **Caching**:
    - **Redis**: ⚡ For distributed caching.
    - **Hybrid Cache**: 🧲 Combines memory and distributed caching (available in .NET 9).

### 📦 Packages
- **FluentValidation**: 📝 For validating request inputs efficiently.
- **Stripe**: 💳 For handling payment processing.
- **Serilog**: 📈 For structured and extensible logging.
- **Hangfire**: ⏳ For managing background jobs with a user-friendly UI.
- **Health Checks**: 🏥 To monitor SQL, background jobs, and application health.
- **Mapster**: 🔄 For lightweight object-to-object mapping.
- **Mailjet**: 📧 For reliable email services.

---

## 🧑‍💻 Key Features

### **Account Management**
- 🧑‍💼 **Get Info**: Retrieve the authenticated user's account information.
- ✍️ **Update Info**: Update the authenticated user's account details.
- 🔒 **Change Password**: Change the password securely.
- 🎓 **Get Enrollment Courses**: Retrieve all the courses the student is enrolled in.
- 💳 **Get Payment History**: Retrieve all payments made by the user.

### **Student Management**
- 👤 **Get Student by ID**: Retrieve student details by their unique ID.
- 📜 **Get All Students**: Fetch all students in the system.
- ✍️ **Update Student**: Update an existing student's details.
- 🔄 **Toggle Student Status**: Toggle the student’s active/inactive status.
- 🗑️ **Delete Student**: Remove a student from the system.

### **Instructor Management**
- 👤 **Get Instructor by ID**: Retrieve instructor details by their unique ID.
- 📜 **Get All Instructors**: Fetch all instructors in the system.
- ✍️ **Update Instructor**: Update an instructor's information.
- 🔄 **Toggle Instructor Status**: Activate or deactivate an instructor’s status.

### **User Management**
- 👥 **Get All Users**: Fetch all users in the system.
- 👤 **Get User by ID**: Retrieve a user's details by their ID.
- ➕ **Add User**: Add a new user (student, instructor, admin).
- ✍️ **Update User**: Update a user's information.
- 🔄 **Toggle User Status**: Toggle a user’s status (active/inactive).
- 🔓 **Unlock User**: Unlock a user's account if it was locked.

### **Authentication Management**
- 🔑 **Login**: Authenticate a user and receive a token.
- 🔄 **Refresh Token**: Refresh an expired token.
- ❌ **Revoke Refresh Token**: Revoke an existing refresh token.
- 🧑‍🎓 **Register Student**: Register a new student.
- 🧑‍🏫 **Register Instructor**: Register a new instructor.
- ✅ **Confirm Email**: Confirm a new user’s email address.
- 🔁 **Resend Confirmation Email**: Resend email confirmation.
- 🧠 **Forget Password**: Initiate password reset.
- 🔒 **Reset Password**: Reset the user’s password using a reset code.

### **Roles Management**
- 📜 **Get All Roles**: Retrieve all roles, including disabled ones.
- 🧑‍💼 **Get Role By ID**: Get details for a specific role.
- ➕ **Add Role**: Add a new role.
- 📝 **Update Role**: Update an existing role.
- ⚙️ **Toggle Role Status**: Toggle a role’s status (enable/disable).

### **Category Management**
- 🛠️ **Get Category by ID**: Get a specific category.
- 📜 **Get All Categories**: Retrieve all categories.
- ➕ **Create Category**: Create a new category (admin/instructor access).
- ✏️ **Update Category**: Update an existing category (admin/instructor access).
- ⚙️ **Toggle Category Status**: Toggle a category’s status (admin/instructor access).

### **Lesson Management**
- 🛠️ **Get Lesson by ID**: Retrieve a lesson by its ID.
- 📜 **Get All Lessons**: Fetch all available lessons.
- ➕ **Create Lesson**: Add a new lesson (admin/instructor access).
- ✏️ **Update Lesson**: Update a lesson’s details (admin/instructor access).
- ⚙️ **Toggle Lesson Status**: Toggle lesson status (admin/instructor access).
- 💬 **Count Comments for Lesson**: Retrieve comment count for a lesson.

### **Comment Management**
- 🛠️ **Get Comment by ID**: Retrieve a specific comment by its ID.
- 📜 **Get All Comments**: Fetch all comments.
- ➕ **Create Comment**: Add a new comment.
- ✏️ **Update Comment**: Update an existing comment.
- ⚙️ **Toggle Comment Status**: Toggle comment status (admin/instructor access).

### **Course Management**
- 🛠️ **Get Course by ID**: Retrieve a specific course by ID.
- 📜 **Get All Courses**: Retrieve all courses with filters.
- ➕ **Create Course**: Add a new course (admin access).
- ✏️ **Update Course**: Modify an existing course (admin access).
- ⚙️ **Toggle Course Status**: Toggle course status (admin access).
- 🧑‍🏫 **Get Courses by Instructor ID**: Retrieve courses assigned to an instructor.
- 📚 **Get Courses by Category ID**: Retrieve courses under a specific category.
- 📊 **Get Course Structure**: Fetch the structure of all courses.
- 🏫 **Get Course Structure by ID**: Retrieve structure for a specific course.
- 📈 **Get Course Enrollment Counts**: Get enrollment stats.
- 💵 **Get Course Refunded Counts**: Retrieve refunded course counts.

### **Document Management**
- 🛠️ **Get Document by ID**: Retrieve a document by its ID.
- 📜 **Get All Documents**: Fetch all documents.
- ➕ **Create Document**: Add a new document (admin/instructor access).
- ✏️ **Update Document**: Modify an existing document (admin/instructor access).
- ⚙️ **Toggle Document Status**: Toggle document status (admin/instructor access).

### **Section Management**
- 🛠️ **Get Section by ID**: Retrieve a section by its unique ID (Admin/Instructor access).
- 📜 **Get All Sections**: Retrieve all sections (Admin/Instructor access).
- 📚 **Get All Sections with Lessons**: Retrieve all sections along with their lessons (Admin/Instructor access).
- ➕ **Create Section**: Add a new section (Admin access).
- ✏️ **Update Section**: Update an existing section's details (Admin/Instructor access).
- 🔄 **Toggle Section Status**: Activate or deactivate a section's status (Admin access).

### **Quiz Management**
- 🛠️ **Get Quiz by ID**: Retrieve a quiz by its unique ID.
- 📜 **Get All Quizzes**: Retrieve all quizzes.
- ➕ **Create Quiz**: Add a new quiz (Admin/Instructor access).
- ✏️ **Update Quiz**: Update an existing quiz's details (Admin/Instructor access).
- 🔄 **Toggle Quiz Status**: Activate or deactivate a quiz's status (Admin/Instructor access).

### **Quiz Attempt Management**
- 🛠️ **Get Quiz Attempt by ID**: Retrieve a quiz attempt by its unique ID.
- 📜 **Get All Quiz Attempts**: Fetch all quiz attempts (Admin/Instructor access).
- ➕ **Create Quiz Attempt**: Record a new quiz attempt.
- ✏️ **Update Quiz Attempt**: Update an existing quiz attempt's details.
- 🔄 **Toggle Quiz Attempt Status**: Activate or deactivate a quiz attempt's status (Admin/Instructor access).

### **Question Management**
- 🛠️ **Get Question by ID**: Retrieve a specific question by its ID.
- 📜 **Get All Questions**: Fetch all questions available.
- ➕ **Create Question**: Add a new question (Admin/Instructor access).
- ✏️ **Update Question**: Modify an existing question (Admin/Instructor access).
- 🔄 **Toggle Question Status**: Change the status of a question (Admin/Instructor access).


### **Answer Management**
- 🛠️ **Get Answer by ID**: Retrieve a specific answer by its ID.
- 📜 **Get All Answers**: Fetch all available answers.
- ➕ **Create Answer**: Add a new answer (Admin/Instructor access).
- ✏️ **Update Answer**: Modify an existing answer (Admin/Instructor access).
- 🔄 **Toggle Answer Status**: Change the status of an answer (Admin/Instructor access).
- ❌ **Delete Answer**: Remove an answer (Admin/Instructor access).

### **Enrollment Management**
- 🛠️ **Get Enrollment by ID**: Retrieve a specific enrollment by its ID (Student role).
- 📜 **Get All Enrollments**: Fetch all available enrollments.
- ➕ **Create Enrollment**: Add a new enrollment (Admin access).
- ✏️ **Update Enrollment**: Modify an existing enrollment (Admin access).
- 🔄 **Refund Payment**: Process a payment refund for an enrollment (Admin access).
- 🔄 **Change Enrollment Owner**: Transfer ownership of an enrollment (Admin access).
- 🔄 **Replace with New Course**: Replace an enrollment with a new course (Admin access).

