using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
// important reference http://habrahabr.ru/post/150859/
namespace ConsoleApplication1
{
    [TestFixture]
    class Test
    {
         //state verification tests 

        [Test]
        public static void MyTest1()
        {
            // Mock.Of возвращает саму зависимость (прокси-объект), а не мок-объект.
            // Следующий код означает, что при вызове GetCurrentDirectory()
            // мы получим "D:\\Temp"
            ILoggerDependency loggerDependency =
                Mock.Of<ILoggerDependency>(d => d.GetCurrentDirectory() == "D:\\Temp");
            var currentDirectory = loggerDependency.GetCurrentDirectory();

            Assert.That(currentDirectory, Is.EqualTo("D:\\Temp"));


        }

        [Test]
        public static void MyTest2()
        {
            // Для любого аргумента метода GetDirectoryByLoggerName вернуть "C:\\Foo".
            ILoggerDependency loggerDependency = Mock.Of<ILoggerDependency>(
                ld => ld.GetDirectoryByLoggerName(It.IsAny<string>()) == "C:\\Foo");

            string directory = loggerDependency.GetDirectoryByLoggerName("anything");

            Assert.That(directory, Is.EqualTo("C:\\Foo"));

        }

        [Test]
        public static void MyTest3()
        {
            // Инициализируем заглушку таким образом, чтобы возвращаемое значение
            // метода GetDirrectoryByLoggerName зависело от аргумента метода.
            // Код аналогичен заглушке вида:
            // public string GetDirectoryByLoggername(string s) { return "C:\\" + s; }
            Mock<ILoggerDependency> stub = new Mock<ILoggerDependency>();

            stub.Setup(ld => ld.GetDirectoryByLoggerName(It.IsAny<string>()))
                .Returns<string>(name => "C:\\" + name);

            string loggerName = "SomeLogger";
            ILoggerDependency logger = stub.Object;
            string directory = logger.GetDirectoryByLoggerName(loggerName);

            Assert.That(directory, Is.EqualTo("C:\\" + loggerName));


        }

        [Test]
        public static void MyTest4()
        {
            // Объединяем заглушки разных методов с помощью логического «И»
            ILoggerDependency logger =
                Mock.Of<ILoggerDependency>(
                    d => d.GetCurrentDirectory() == "D:\\Temp" &&
                            d.DefaultLogger == "DefaultLogger" &&
                            d.GetDirectoryByLoggerName(It.IsAny<string>()) == "C:\\Temp");

            Assert.That(logger.GetCurrentDirectory(), Is.EqualTo("D:\\Temp"));
            Assert.That(logger.DefaultLogger, Is.EqualTo("DefaultLogger"));
            Assert.That(logger.GetDirectoryByLoggerName("CustomLogger"), Is.EqualTo("C:\\Temp"));

        }

        [Test]
        public static void MyTest5()
        {
            var stub = new Mock<ILoggerDependency>();
            stub.Setup(ld => ld.GetCurrentDirectory()).Returns("D:\\Temp");
            stub.Setup(ld => ld.GetDirectoryByLoggerName(It.IsAny<string>())).Returns("C:\\Temp");
            stub.SetupGet(ld => ld.DefaultLogger).Returns("DefaultLogger");

            ILoggerDependency logger = stub.Object;

            Assert.That(logger.GetCurrentDirectory(), Is.EqualTo("D:\\Temp"));
            Assert.That(logger.DefaultLogger, Is.EqualTo("DefaultLogger"));
            Assert.That(logger.GetDirectoryByLoggerName("CustomLogger"), Is.EqualTo("C:\\Temp"));


        }


        //behavior verification

        [Test]
        public static void MyTest6()
        {
            //var mock = new Mock<ILogWriter>();
            //var logger = new Logger(mock.Object);

            //logger.WriteLine("Hello, logger!");

            //// Проверяем, что вызвался метод Write нашего мока с любым аргументом
            //mock.Verify(lw => lw.Write(It.IsAny<string>()));

            var mock = new Mock<ILogWriter>();
            var logger = new Logger(mock.Object);

            logger.WriteLine("Hello, logger!");

            // Проверка вызова метода ILogWriter.Write с заданным аргументами

            mock.Verify(lw => lw.Write("Hello, logger!"));
        }
            
        [Test]
        public static void MyTest7()
        {
            var mock = new Mock<ILogWriter>();
            var logger = new Logger(mock.Object);

            logger.WriteLine("Hello, logger!");

            // Проверка вызова метода ILogWriter.Write с заданным аргументами

            mock.Verify(lw => lw.Write("Hello, logger!"));

        }

        [Test]
        public static void MyTest8()
        {
            var mock = new Mock<ILogWriter>();
            var logger = new Logger(mock.Object);

            logger.WriteLine("Hello, logger!");

            //Проверка того, что метод ILogWriter.Write вызвался в точности один раз (ни больше, ни меньше)
            mock.Verify(lw => lw.Write(It.IsAny<string>()),
    Times.Once());

        }
            
        [Test]
        public static void MyTest9()
        {
            var mock = new Mock<ILogWriter>();
            mock.Setup(lw => lw.Write(It.IsAny<string>()));

            var logger = new Logger(mock.Object);
            logger.WriteLine("Hello, logger!");

            // Мы не передаем методу Verify никаких дополнительных параметров.
            // Это значит, что будут использоваться ожидания установленные
            // с помощью mock.Setup
            mock.Verify();
        }
            
        [Test]
        public static void MyTest10()
        {
            //создать мок-объект и задать ожидаемое поведение с помощью методов Setup 
            //и проверять все эти допущения путем вызова одного метода Verify(). 

            var mock = new Mock<ILogWriter>();
            mock.Setup(lw => lw.Write(It.IsAny<string>()));
            mock.Setup(lw => lw.SetLogger(It.IsAny<string>()));

            var logger = new Logger(mock.Object);
            logger.WriteLine("Hello, logger!");

            mock.Verify();

        }


    }
}
