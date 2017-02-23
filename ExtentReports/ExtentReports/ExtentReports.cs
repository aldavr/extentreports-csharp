﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;

using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Model;

namespace AventStack.ExtentReports
{
    /// <summary>
    /// <para>
    /// The ExtentReports report client for starting reporters and building reports. For most applications,
    /// you should have one ExtentReports instance for the entire run session. 
    /// </para>
    /// 
    /// <para>
    /// ExtentReports itself does not build any reports, but allows reporters to access information, which in
    /// turn build the said reports.
    /// </para>
    /// </summary>
    public class ExtentReports : Report
    {
        /// <summary>
        /// Attach a <see cref="IExtentReporter"/> reporter, allowing it to access all started tests, nodes and logs 
        /// </summary>
        /// <param name="reporter"><see cref="IExtentReporter" /></param>
        public void AttachReporter(params IExtentReporter[] reporter)
        {
            reporter.ToList().ForEach(x => Attach(x));
        }

        /// <summary>
        /// Creates a BDD-style test with description representing one of the <see cref="IGherkinFormatterModel"/> 
        /// classes such as:
        /// 
        /// <list type="bullet">
        /// <item><see cref="Feature"/></item>
        /// <item><see cref="Background"/></item>
        /// <item><see cref="Scenario"/></item>
        /// <item><see cref="Given"/></item>
        /// <item><see cref="When"/></item>
        /// <item><see cref="Then"/></item>
        /// <item><see cref="And"/></item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">A <see cref="IGherkinFormatterModel"/> type</typeparam>
        /// <param name="name">Test name</param>
        /// <param name="description">A short description</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ExtentTest CreateTest<T>(string name, string description = null) where T : IGherkinFormatterModel
        {
            Type type = typeof(T);
            var obj = (IGherkinFormatterModel)Activator.CreateInstance(type);

            var extentTest = new ExtentTest(this, obj, name, description);
            CreateTest(extentTest.GetModel());

            return extentTest;
        }

        /// <summary>
        /// Creates a test with description
        /// </summary>
        /// <param name="name">Test name</param>
        /// <param name="description">A short description</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ExtentTest CreateTest(string name, string description = null)
        {
            var extentTest = new ExtentTest(this, name, description);
            CreateTest(extentTest.GetModel());

            return extentTest;
        }

        /// <summary>
        /// Creates a BDD-style test with description using name of the Gherkin model such as:
        /// 
        /// <list type="bullet">
        /// <item><see cref="Feature"/></item>
        /// <item><see cref="Background"/></item>
        /// <item><see cref="Scenario"/></item>
        /// <item><see cref="Given"/></item>
        /// <item><see cref="When"/></item>
        /// <item><see cref="Then"/></item>
        /// <item><see cref="And"/></item>
        /// </list>
        /// </summary>
        /// <param name="gherkinKeyword">Name of the <see cref="GherkinKeyword"/></param>
        /// <param name="name">Test name</param>
        /// <param name="description">A short description</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ExtentTest CreateTest(GherkinKeyword gherkinKeyword, string name, string description = null)
        {
            var extentTest = CreateTest(name, description);
            extentTest.GetModel().BehaviorDrivenType = gherkinKeyword.GetModel();
            return extentTest;
        }

        /// <summary>
        /// Writes test information from the started reporters to their output view
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public new void Flush()
        {
            base.Flush();
        }

        /// <summary>
        /// Adds any applicable system information to all started reporters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddSystemInfo(string name, string value)
        {
            SystemAttribute sa = new SystemAttribute(name, value);
            AddSystemAttribute(sa);
        }

        /// <summary>
        /// Adds logs from test framework tools to the test-runner logs view (if available in the reporter)
        /// </summary>
        /// <param name="log"></param>
        public void AddTestRunnerLogs(string log)
        {
            AddTestRunnerLog(log);
        }

        /// <summary>
        /// Adds logs from test framework tools to the test-runner logs view (if available in the reporter)
        /// </summary>
        /// <param name="log"></param>
        public void AddTestRunnerLogs(string[] log)
        {
            log.ToList().ForEach(x => AddTestRunnerLog(x));
        }
    }
}
