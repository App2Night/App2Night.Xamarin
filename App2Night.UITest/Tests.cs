using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace App2Night.UITest
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void NewTest()
        {
            app.Tap(x => x.Marked("UsernameInput"));
            app.EnterText(x => x.Marked("UsernameInput"), "test");
            app.Tap(x => x.Marked("PasswordInput"));
            app.EnterText(x => x.Marked("PasswordInput"), "test");
            app.Tap(x => x.Text("").Index(1));
            app.Tap(x => x.Marked("SignUpSwitch"));
            app.Tap(x => x.Marked("EmailInput"));
            app.EnterText(x => x.Marked("EmailInput"), "test@test.test");
            app.Tap(x => x.Class("Platform_DefaultRenderer"));
            app.Tap(x => x.Marked("AcceptAgbSwitch"));
            app.Tap(x => x.Marked("SignUpSwitch"));
            app.Tap(x => x.Marked("Submit"));
        }

        [Test]
        public void NewTest1()
        {
            app.SwipeLeftToRight();
            app.PressEnter();
            app.ScrollDown();
            app.ScrollUp();
            app.ScrollDown();
            app.ScrollDown();
            app.Tap(x => x.Class("CustomScrollViewRenderer"));
            app.ScrollUp();
            app.ScrollUp();
            app.ScrollDown();
        }
    }
}

