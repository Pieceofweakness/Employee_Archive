using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace Employee_archive.tests
{
    [TestClass]
    public class DatabaseHelperTests
    {
        private readonly Mock<IDatabaseHelper> mockDb;

        public DatabaseHelperTests()
        {
            mockDb = new Mock<IDatabaseHelper>();
        }


        [TestMethod]
        public void GetAllEmployees_ShouldReturnMockData()
        {
            //arrange
            var expectedEmployees = new List<Employee>
            {
                new Employee { ID_employee = 1, Full_Name = "Иванов И.И.", RoleName = "Кассир" },
                new Employee { ID_employee = 2, Full_Name = "Петров П.П.", RoleName = "Менеджер" }
            };

            mockDb.Setup(x=>x.GetAllEmployees()).Returns(expectedEmployees);


            //act
            var result = mockDb.Object.GetAllEmployees();

            //assert

            Assert.AreEqual(2,result.Count);
            Assert.AreEqual(expectedEmployees[0].ID_employee, result[0].ID_employee);
            Assert.AreEqual(expectedEmployees[0].Full_Name, result[0].Full_Name);
            Assert.AreEqual(expectedEmployees[1].ID_employee, result[1].ID_employee);
            Assert.AreEqual(expectedEmployees[1].Full_Name, result[1].Full_Name);

        }

        [TestMethod]
        public void Authenticate_WithValidData_ShouldReturnAdmin()
        {
            //arrange
            var expectedAdmin = new Administrator
            {
                ID_Admin = 1,
                Full_Name = "Admin",
                Login = "admin"
            };
            mockDb.Setup(x =>x.Authenticate("admin","1234")).Returns(expectedAdmin);


            //act
            var result = mockDb.Object.Authenticate("admin","1234");

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Admin",result.Full_Name);
            Assert.AreEqual(1,result.ID_Admin);

        }

        [TestMethod]
        public void AddEmployee_ShouldreturnTrue_WhenSeccesful()
        {
            //arrange
            var employee = new Employee { Full_Name = "Test" };
            mockDb.Setup(x=>x.AddEmployee(It.IsAny<Employee>())).Returns(true);

            //act
            var result = mockDb.Object.AddEmployee(employee);

            //assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddEmployee_ShouldReturnFalse_WhenFailed()
        {
            //arrange
            var employee = new Employee { Full_Name = "" };
            mockDb.Setup(x => x.AddEmployee(It.IsAny<Employee>())).Returns(false);

            //act
            var result = mockDb.Object.AddEmployee(employee);

            //assert
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void DeleteEmployees_ShouldReturnTrue_WhenSuccessful()
        {
            //arrange
            var employee = new Employee { ID_employee = 1 };
            mockDb.Setup(x => x.DeleteEmployee(It.IsAny<Employee>())).Returns(true);

            //act
            var result = mockDb.Object.DeleteEmployee(employee);

            //assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteEmployee_ShouldReturnFalse_WhenEmployeeNotFound()
        {
            //arrange
            var employee = new Employee { ID_employee = 9999 };
            mockDb.Setup(x => x.DeleteEmployee(It.IsAny<Employee>())).Returns(false);

            //act
            var result = mockDb.Object.DeleteEmployee(employee);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateEmployee_ShouldReturnTrue_WhenSuccessful()
        {
            //arrange
            var employee = new Employee
            {
                ID_employee = 1,
                Full_Name = "Обновленное Имя",
                Born_date = "15-05-1990",
                Phone = "+7 (999) 111-11-11",
                Role = 2,
                Work_days = 45
            };

            mockDb.Setup(x => x.UpdateEmployee(It.IsAny<Employee>())).Returns(true);

            //act
            var result = mockDb.Object.UpdateEmployee(employee);

            //assert
            Assert.IsTrue(result);
            mockDb.Verify(x=>x.UpdateEmployee(employee),Times.Once);
        }

        [TestMethod]
        public void UpdateEmployee_ShouldReturnFalse_WhenEmployeeNotFound()
        {
            //arrange
            var employee = new Employee { ID_employee = 9999, Full_Name = "Несуществующий" };
            mockDb.Setup(x => x.UpdateEmployee(It.IsAny<Employee>())).Returns(false);

            //act
            var result = mockDb.Object.UpdateEmployee(employee);

            //assert
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void GetAllRoles_ShouldReturnMockData()
        {
            //arrange
            var expectedRoles = new List<Role>
            {
                new Role { ID_Role = 1, Name_Role = "Разработчик" },
                new Role { ID_Role = 2, Name_Role = "Менеджер" },
                new Role { ID_Role = 3, Name_Role = "Аналитик" },
                new Role { ID_Role = 4, Name_Role = "Тестировщик" }
            };

            mockDb.Setup(x => x.GetAllRoles()).Returns(expectedRoles);


            //act
            var result = mockDb.Object.GetAllRoles();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("Разработчик", result[0].Name_Role);
            Assert.AreEqual(1, result[0].ID_Role);
            Assert.AreEqual("Менеджер", result[1].Name_Role);
            Assert.AreEqual(2, result[1].ID_Role);
        }

        [TestMethod]
        public void GetTotalEmployeesCount_ShouldReturnCorrectCount()
        {
            //arrange
            var expectedCount = 12;
            mockDb.Setup(x => x.GetTotalEmployeesCount()).Returns(expectedCount);

            //act
            var result = mockDb.Object.GetTotalEmployeesCount();

            // assert
            Assert.AreEqual(expectedCount, result);
            mockDb.Verify(x => x.GetTotalEmployeesCount(), Times.Once);
        }

        [TestMethod]
        public void GetAverageWorkDays_ShouldReturnCorrectAverange()
        {
            //arrange
            var expectedAverage = 150.5;
            mockDb.Setup(x => x.GetAverageWorkDays()).Returns(expectedAverage);

            // act
            var result = mockDb.Object.GetAverageWorkDays();

            // assert
            Assert.AreEqual(expectedAverage, result);
            mockDb.Verify(x => x.GetAverageWorkDays(), Times.Once);
        }
    }
}
