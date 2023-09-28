using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DesktopApplication;

// public class StudentService
// {
//     private DataRepository _dataRepository;
//
//     public StudentService(DataRepository dataRepository)
//     {
//         _dataRepository = dataRepository;
//     }
//
//     public List<Student> GetAllStudents()
//     {
//         return _dataRepository.Students;
//     }
//
//     public void AddStudent(string studentFullName)
//     {
//
//     }
//
//     // public void UpdateStudent(Student student)
//     // {
//     //     _dataRepository.Entry(student).State = EntityState.Modified;
//     //     _dataRepository.SaveChanges();
//     // }
//
//     // public void DeleteStudent(int studentId)
//     // {
//     //     var student = _dataRepository.Students.Find(studentId);
//     //     if (student != null)
//     //     {
//     //         _dataRepository.Students.Remove(student);
//     //         _dataRepository.SaveChanges();
//     //     }
//     // }
// }