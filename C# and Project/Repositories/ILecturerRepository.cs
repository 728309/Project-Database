﻿using C__and_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace C__and_Project.Repositories
{
    public interface ILecturerRepository
    {
        List<Lecturer> GetAllLecturers();
        Lecturer? GetLecturerByID(int LecturerID);
        void AddLecturer(Lecturer lectuer);
        void UpdateLecturer(Lecturer lecturer);
        void DeleteLecturer(Lecturer lecturer);
        List<Lecturer> GetSupervisorsByActivityId(int activityId);
    }
}