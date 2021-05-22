using HallOfFame.Controllers;
using HallOfFame.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

using System.Linq;
using Nest;

namespace HallOfFame.Tests
{
    public class HallOfFameTests
    {

        [Fact]
        public void IndexReturnsAViewResultWithAListOfUsers()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTestPersons());
            var controller = new PersonController((PersonContext)mock.Object);

            // Act
            var result = controller.GetPersons();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Person>>(
                viewResult.Model);
            Assert.Equal(GetTestPersons().Count, model.Count());
        }
        private List<Person> GetTestPersons()
        {

            var skill1 = new Skill { SkillId = 1, Level = 5, Name = "Fly" };
            var skill2 = new Skill { SkillId = 1, Level = 7, Name = "Swim" };
            var skill3 = new Skill { SkillId = 2, Level = 7, Name = "Tok" };
            var skill4 = new Skill { SkillId = 3, Level = 7, Name = "Map" };
            var skill5 = new Skill { SkillId = 4, Level = 7, Name = "Jek" };
            var skill6 = new Skill { SkillId = 4, Level = 7, Name = "AS" };

            var users = new List<Person>
            {
                new Person { Id=1, Name="Tom", DisplayName="Thomas", Skills = {skill1,skill2 }},
                new Person { Id=2, Name="Alice", DisplayName= "Alice S.", Skills = {skill3 }},
                new Person { Id=3, Name="Sam", DisplayName="Samuel", Skills = { skill4}},
                new Person { Id=4, Name="Kate", DisplayName="Katerina" , Skills = {skill5,skill6 } }
            };
            return users;
        }
    }
}
