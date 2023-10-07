// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore;
//
// namespace DesktopApplication;
//
// public class StudentService
// {
//     private UniversityDbContext _dbContext;
//
//     public StudentService(UniversityDbContext dbContext)
//     {
//         _dbContext = dbContext;
//     }
//
//     public DbSet<Student> GetAllStudents()
//     {
//         return _dbContext.Students;
//     }
//
//     public void AddStudent(string studentFullName)
//     {
//     
//     }
//     
//     public void UpdateStudent(Student student)
//     {
//         _dbContext.Entry(student).State = EntityState.Modified;
//         _dbContext.SaveChanges();
//     }
//     
//     public void DeleteStudent(int studentId)
//     {
//         var student = _dbContext.Students.Find(studentId);
//         if (student != null)
//         {
//             _dbContext.Students.Remove(student);
//             _dbContext.SaveChanges();
//         }
//     }
// }