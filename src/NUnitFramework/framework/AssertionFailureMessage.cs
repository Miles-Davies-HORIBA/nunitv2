/********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************/
using System;
using System.Text;

namespace NUnit.Framework
{
	/// <summary>
	/// Summary description for AssertionFailureMessage.
	/// </summary>
	public class AssertionFailureMessage
	{
		protected AssertionFailureMessage() 
		{}

        /// <summary>
        /// Number of characters before a highlighted position before
        /// clipping will occur.  Clipped text is replaced with an
        /// elipses "..."
        /// </summary>
        static protected int PreClipLength
        {
            get
            {
                return 35;
            }
        }

        /// <summary>
        /// Number of characters after a highlighted position before
        /// clipping will occur.  Clipped text is replaced with an
        /// elipses "..."
        /// </summary>
        static protected int PostClipLength
        {
            get
            {
                return 35;
            }
        }   

        /// <summary>
        /// Called to test if the position will cause clipping
        /// to occur in the early part of a string.
        /// </summary>
        /// <param name="iPosition"></param>
        /// <returns></returns>
        static private bool IsPreClipped( int position )
        {
            if( position > PreClipLength )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Called to test if the position will cause clipping
        /// to occur in the later part of a string past the
        /// specified position.
        /// </summary>
        /// <param name="sString"></param>
        /// <param name="iPosition"></param>
        /// <returns></returns>
        static private bool IsPostClipped( string sString, int iPosition )
        {
            if( sString.Length - iPosition > PostClipLength )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Property called to insert newline characters into a string
        /// </summary>
        static private string NewLine
        {
            get
            {
                return "\r\n";
            }
        }

        /// <summary>
        /// Renders up to M characters before, and up to N characters after
        /// the specified index position.  If leading or trailing text is
        /// clipped, and elipses "..." is added where the missing text would
        /// be.
        /// 
        /// Clips strings to limit previous or post newline characters,
        /// since these mess up the comparison
        /// </summary>
        /// <param name="sString"></param>
        /// <param name="iPosition"></param>
        /// <returns></returns>
        static protected string ClipAroundPosition( string sString, int iPosition )
        {
            if( null == sString || 0 == sString.Length )
            {
                return "";
            }

            return BuildBefore( sString, iPosition ) + BuildAfter(  sString, iPosition );
        }

        /// <summary>
        /// Clips the string before the specified position, and appends
        /// ellipses (...) to show that clipping has occurred 
        /// </summary>
        /// <param name="sString"></param>
        /// <param name="iPosition"></param>
        /// <returns></returns>
        static protected string PreClip( string sString, int iPosition )
        {
            return "..." + sString.Substring( iPosition - PreClipLength, PreClipLength );
        }

        /// <summary>
        /// Clips the string after the specified position, and appends
        /// ellipses (...) to show that clipping has occurred 
        /// </summary>
        /// <param name="sString"></param>
        /// <param name="iPosition"></param>
        /// <returns></returns>
        static protected string PostClip( string sString, int iPosition )
        {
            return sString.Substring( iPosition, PostClipLength ) + "...";
        }

        /// <summary>
        /// Builds the first half of a string, limiting the number of
        /// characters before the position, and removing newline
        /// characters.  If the leading string is truncated, the
        /// ellipses (...) characters are appened.
        /// </summary>
        /// <param name="sString"></param>
        /// <param name="iPosition"></param>
        /// <returns></returns>
        static private string BuildBefore( string sString, int iPosition )
        {
            if( IsPreClipped(iPosition) )
            {
                return PreClip( sString, iPosition );
            }
            return sString.Substring( 0, iPosition );
        }

        /// <summary>
        /// Builds the last half of a string, limiting the number of
        /// characters after the position, and removing newline
        /// characters.  If the string is truncated, the
        /// ellipses (...) characters are appened.
        /// </summary>
        /// <param name="sString"></param>
        /// <param name="iPosition"></param>
        /// <returns></returns>
        static private string BuildAfter( string sString, int iPosition )
        {
            if( IsPostClipped(sString, iPosition) )
            {
                return PostClip( sString, iPosition );
            }
            return sString.Substring( iPosition );
        }

        /// <summary>
        /// Text that is rendered for the expected value
        /// </summary>
        /// <returns></returns>
        static protected string ExpectedText()
        {
            return "expected:<";
        }

        /// <summary>
        /// Text rendered for the actual value.  This text should
        /// be the same length as the Expected text, so leading
        /// spaces should pad this string to ensure they match.
        /// </summary>
        /// <returns></returns>
        static protected string ButWasText()
        {
            return " but was:<";
        }

        /// <summary>
        /// Raw line that communicates the expected value, and the actual value
        /// </summary>
        /// <param name="sbOutput"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        static protected void AppendExpectedAndActual( StringBuilder sbOutput, Object expected, Object actual )
        {
            sbOutput.Append( NewLine );
            sbOutput.Append( ExpectedText() );
            sbOutput.Append( (expected != null) ? expected : "(null)" );
            sbOutput.Append( ">" );
            sbOutput.Append( NewLine );
            sbOutput.Append( ButWasText() );
            sbOutput.Append( (actual != null) ? actual : "(null)" );
            sbOutput.Append( ">" );
        }

        /// <summary>
        /// Draws a marker under the expected/actual strings that highlights
        /// where in the string a mismatch occurred.
        /// </summary>
        /// <param name="sbOutput"></param>
        /// <param name="iPosition"></param>
        static protected void AppendPositionMarker( StringBuilder sbOutput, int iPosition )
        {
            sbOutput.Append( new String( '-', ButWasText().Length ) );
            if( iPosition > 0 )
            {
                sbOutput.Append( new string( '-', iPosition ) );
            }
            sbOutput.Append( "^" );
        }

        /// <summary>
        /// Tests two objects to determine if they are strings.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        static protected bool InputsAreStrings( Object expected, Object actual )
        {
            if( null != expected  &&
                null != actual    &&
                expected is string &&
                actual   is string )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tests if two strings are different lengths.
        /// </summary>
        /// <param name="sExpected"></param>
        /// <param name="sActual"></param>
        /// <returns>True if string lengths are different</returns>
        static protected bool LengthsDifferent( string sExpected, string sActual )
        {
            if( sExpected.Length != sActual.Length )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Used to construct a message when the lengths of two strings are
        /// different.  Also includes the strings themselves, to allow them
        /// to be compared visually.
        /// </summary>
        /// <param name="sbOutput"></param>
        /// <param name="sExpected"></param>
        /// <param name="sActual"></param>
        static protected void BuildLengthsDifferentMessage( StringBuilder sbOutput, string sExpected, string sActual )
        {
            BuildContentDifferentMessage( sbOutput, sExpected, sActual );
        }

        /// <summary>
        /// Reports the length of two strings that are different lengths
        /// </summary>
        /// <param name="sbOutput"></param>
        /// <param name="sExpected"></param>
        /// <param name="sActual"></param>
        static protected void BuildStringLengthDifferentReport( StringBuilder sbOutput, string sExpected, string sActual )
        {
            sbOutput.Append( "String lengths differ.  Expected length=" );
            sbOutput.Append( sExpected.Length );
            sbOutput.Append( ", but was length=" );
            sbOutput.Append( sActual.Length );
            sbOutput.Append( "." );
            sbOutput.Append( NewLine );
        }

        /// <summary>
        /// Reports the length of two strings that are the same length
        /// </summary>
        /// <param name="sbOutput"></param>
        /// <param name="sExpected"></param>
        /// <param name="sActual"></param>
        static protected void BuildStringLengthSameReport(  StringBuilder sbOutput, string sExpected, string sActual )
        {
            sbOutput.Append( "String lengths are both " );
            sbOutput.Append( sExpected.Length );
            sbOutput.Append( "." );
            sbOutput.Append( NewLine );
        }

        /// <summary>
        /// Reports whether the string lengths are the same or different, and
        /// what the string lengths are.
        /// </summary>
        /// <param name="sbOutput"></param>
        /// <param name="sExpected"></param>
        /// <param name="sActual"></param>
        static protected void BuildStringLengthReport( StringBuilder sbOutput, string sExpected, string sActual )
        {
            if( sExpected.Length != sActual.Length )
            {
                BuildStringLengthDifferentReport( sbOutput, sExpected, sActual );
            }
            else
            {
                BuildStringLengthSameReport( sbOutput, sExpected, sActual );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbOutput"></param>
        /// <param name="sExpected"></param>
        /// <param name="sActual"></param>
        /// <param name="iPosition"></param>
        static private void BuildContentDifferentAtPosition( StringBuilder sbOutput, string sExpected, string sActual, int iPosition )
        {
            BuildStringLengthReport( sbOutput, sExpected, sActual );

            sbOutput.Append( "Strings differ at index " );
            sbOutput.Append( iPosition );
            sbOutput.Append( "." );
            sbOutput.Append( NewLine );

            //
            // Clips the strings, then turns any hidden newlines into visible
            // characters by replacing the '\r' into '\\' and 'r' characters,
            // and the '\n' into '\\' and 'n' characters.  Thus the single 
            // character becomes two characters for display.
            //
            string sClippedExpected = ConvertNewlines(ClipAroundPosition( sExpected, iPosition ));
            string sClippedActual   = ConvertNewlines(ClipAroundPosition( sActual,   iPosition ));

            AppendExpectedAndActual( 
                sbOutput, 
                sClippedExpected, 
                sClippedActual );
            sbOutput.Append( NewLine );

            // Add a line showing where they differ.  If the string lengths are
            // different, they start differing just past the length of the 
            // shorter string
            AppendPositionMarker( 
                sbOutput, 
                FindMismatchPosition( sClippedExpected, sClippedActual, 0 ) );
            sbOutput.Append( NewLine );
        }

        /// <summary>
        /// Turns CR or LF into visual indicator to preserve visual marker 
        /// position.
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        static protected string ConvertNewlines( string sInput )
        {
            if( null != sInput )
            {
                sInput = sInput.Replace( "\r", "\\r" );
                sInput = sInput.Replace( "\n", "\\n" );
            }
            return sInput;
        }

        /// <summary>
        /// Shows the position two strings start to differ.  Comparison 
        /// starts at the start index.
        /// </summary>
        /// <param name="sExpected"></param>
        /// <param name="sActual"></param>
        /// <param name="iStart"></param>
        /// <returns>-1 if no mismatch found, or the index where mismatch found</returns>
        static private int FindMismatchPosition( string sExpected, string sActual, int iStart )
        {
            int iLength = Math.Min( sExpected.Length, sActual.Length );
            for( int i=iStart; i<iLength; i++ )
            {
                //
                // If they mismatch at a specified position, report the
                // difference.
                //
                if( sExpected[i] != sActual[i] )
                {
                    return i;
                }
            }
            //
            // Strings have same content up to the length of the shorter string.
            // Mismatch occurs because string lengths are different, so show
            // that they start differing where the shortest string ends
            //
            if( sExpected.Length != sActual.Length )
            {
                return iLength;
            }
            
            //
            // Same strings
            //
            Assertion.Assert( sExpected.Equals( sActual ) );
            return -1;
        }

        /// <summary>
        /// Constructs a message that can be displayed when the content of two
        /// strings are different, but the string lengths are the same.  The
        /// message will clip the strings to a reasonable length, centered
        /// around the first position where they are mismatched, and draw 
        /// a line marking the position of the difference to make comparison
        /// quicker.
        /// </summary>
        /// <param name="sbOutput"></param>
        /// <param name="sExpected"></param>
        /// <param name="sActual"></param>
        static protected void BuildContentDifferentMessage( StringBuilder sbOutput, string sExpected, string sActual )
        {
            //
            // If they mismatch at a specified position, report the
            // difference.
            //
            int iMismatch = FindMismatchPosition( sExpected, sActual, 0 );
            if( -1 != iMismatch )
            {
                BuildContentDifferentAtPosition( 
                    sbOutput, 
                    sExpected, 
                    sActual, 
                    iMismatch );
                return;
            }

            //
            // If the lengths differ, but they match up to the length,
            // show the difference just past the length of the shorter
            // string
            //
            if( sExpected.Length != sActual.Length )
            {
                BuildContentDifferentAtPosition( 
                    sbOutput, 
                    sExpected, 
                    sActual, 
                    Math.Min(sExpected.Length, sActual.Length) );
            }
        }

        /// <summary>
        /// Called to append a message when the input strings are different.
        /// A different message is rendered when the lengths are mismatched,
        /// and when the lengths match but content is mismatched.
        /// </summary>
        /// <param name="sbOutput"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        static private void BuildStringsDifferentMessage( StringBuilder sbOutput, string expected, string actual )
        {
            sbOutput.Append( NewLine );
            if( LengthsDifferent( expected, actual ) )
            {
                BuildLengthsDifferentMessage( sbOutput, expected, actual );
            }
            else
            {
                BuildContentDifferentMessage( sbOutput, expected, actual );
            }
        }

        /// <summary>
        /// Used to create a StringBuilder that is used for constructing
        /// the output message when text is different.  Handles initialization
        /// when a message is provided.  If message is null, an empty
        /// StringBuilder is returned.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        static protected StringBuilder CreateStringBuilder( string message )
        {
            StringBuilder sbOutput;
            if (message != null) 
            {
                sbOutput = new StringBuilder( message );
            }
            else
            {
                sbOutput = new StringBuilder();
            }
            return sbOutput;
        }

        /// <summary>
        /// Called to create a message when two objects have been found to
        /// be unequal.  If the inputs are strings, a special message is
        /// rendered that can help track down where the strings are different,
        /// based on differences in length, or differences in content.
        /// 
        /// If the inputs are not strings, the ToString method of the objects
        /// is used to show what is different about them.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        static public string FormatMessageForFailNotEquals(string message, Object expected, Object actual) 
        {
            StringBuilder sbOutput = CreateStringBuilder( message );
            if( null != message )
            {
                if( message.Length > 0 )
                {
                    sbOutput.Append( " " );
                }
            }

            if( InputsAreStrings( expected, actual ) )
            {
                BuildStringsDifferentMessage( 
                    sbOutput, 
                    (string)expected, 
                    (string)actual );
            }
            else
            {
                AppendExpectedAndActual( sbOutput, expected, actual );
            }
            return sbOutput.ToString();
        }
	}
}
