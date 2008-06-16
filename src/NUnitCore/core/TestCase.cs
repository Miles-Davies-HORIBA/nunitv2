// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

namespace NUnit.Core
{
	using System;
	using System.Collections;
	using System.Reflection;

	/// <summary>
	/// The abstract TestCase class represents a single test case.
	/// In the present implementation, the only derived class is
	/// TestMethod, but we allow for future test cases which are
	/// implemented in other ways.
	/// </summary>
	public abstract class TestCase : Test
	{
		public TestCase( string path, string name ) : base( path, name ) { }

		public TestCase( MethodInfo method ) : base ( method ) { }

		public TestCase( TestName testName ) : base ( testName ) { }

		public override TestResult Run( EventListener listener, ITestFilter filter )
		{
			using( new TestContext() )
			{
				TestResult testResult = new TestResult(this);

				listener.TestStarted( this.TestName );
				long startTime = DateTime.Now.Ticks;

				switch (this.RunState)
				{
					case RunState.Runnable:
					case RunState.Explicit:
						Run(testResult);
						break;
					case RunState.Skipped:
                    default:
                        testResult.Skip(IgnoreReason);
				        break;
					case RunState.NotRunnable:
                        testResult.Invalid( IgnoreReason );
				        break;
					case RunState.Ignored:
                        testResult.Ignore( IgnoreReason );
                        break;
				}

				long stopTime = DateTime.Now.Ticks;
				double time = ((double)(stopTime - startTime)) / (double)TimeSpan.TicksPerSecond;
				testResult.Time = time;

				listener.TestFinished(testResult);
				return testResult;
			}
		}

		public abstract void Run(TestResult result);
	}
}
