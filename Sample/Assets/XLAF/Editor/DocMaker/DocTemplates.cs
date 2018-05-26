using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Document templates.
/// </summary>
public class DocTemplates
{
	#region indexhtml

	public static readonly string indexhtml = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN""
 ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">
<html>
<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/>
<head>
    <title>{0}</title>
    <link rel=""stylesheet"" href=""css.css"" type=""text/css""/>
</head>
<body>

<div id=""container"">

    <div id=""product"">
        <div id=""product_logo""></div>
        <div id=""product_name""><big><b></b></big></div>
        <div id=""product_description""></div>
    </div>

    <div id=""main"">

        <!-- Menu -->
        <div id=""navigation"">
            <br/>
            <h1>{1}</h1>
            <h2>Classes</h2>
            <ul class=""$(kind=='Topics' and '' or 'nowrap'"">
{2}
            </ul>
            <h2>ReadMe</h2>
            <ul class=""$(kind=='Topics' and '' or 'nowrap'"">
                <li><a href=""Readme.md.html"">Readme</a></li>
            </ul>

        </div>

        <div id=""content"">
            <h2>{3}</h2>

            <h2>Classes</h2>
            <table class=""module_list"">
{4}
            </table>
            <h2>Topics</h2>
            <table class=""module_list"">
                <tr>
                    <td class=""name"" nowrap><a href=""Readme.md.html"">Readme.md</a></td>
                    <td class=""summary"">Read me file</td>
                </tr>
            </table>

        </div>
    </div>
    <div id=""about"">
        <i>written by <a href=""https://github.com/SES-xuelan"">XueLan</a></i>
    </div>
</div>
</body>
</html>
";

	#endregion

	#region classesli

	public static readonly string classesli = "                <li><a href=\"{0}\">{1}</a></li>\n";

	#endregion

	#region module list

	public static readonly string modulelistIndex = "                <tr>\n                    <td class=\"name\" nowrap><a href=\"{0}\">{1}</a></td>\n                    <td class=\"summary\">{2}</td>\n                </tr>\n";
	public static readonly string modulelistClass = "                <li><a href=\"../{0}\">{1}</a></li>\n";
	public static readonly string modulelistClassNoLink = "                <li><strong>{0}</strong></li>\n";

	#endregion

	#region function list

	public static readonly string functionList = @"                <tr>
                    <td class=""name"" nowrap><a href=""#{0}"">{1}</a></td>
                    <td class=""summary"">{2}</td>
                </tr>";
	public static readonly string functionInfo = @"                <dt>
                    <a name=""{0}""></a>
                    <strong>{1}</strong>
                </dt>
                <dd>
                    {2}
                    <h3>Parameters:</h3>
                    <ul>
{3}
                    </ul>
					<h3>Returns:</h3>
                    <ul>
{4} {5}
                    </ul>
                </dd>";
	public static readonly string parameter = @"                        <li>{0}<span class=""parameter""> {1}</span>
                            {2}
                        </li>";

	#endregion

	#region variable list
	public static readonly string variableList = @"                <tr>
                    <td class=""name"" nowrap><strong>{0}</strong></td>
                    <td class=""summary"">{1}</td>
                </tr>";
	#endregion

	#region cssfile

	public static readonly string cssfile = @"/* BEGIN RESET

Copyright (c) 2010, Yahoo! Inc. All rights reserved.
Code licensed under the BSD License:
http://developer.yahoo.com/yui/license.html
version: 2.8.2r1
*/
html {
    color: #000;
    background: #FFF;
}
body,div,dl,dt,dd,ul,ol,li,h1,h2,h3,h4,h5,h6,pre,code,form,fieldset,legend,input,button,textarea,p,blockquote,th,td {
    margin: 0;
    padding: 0;
}
table {
    border-collapse: collapse;
    border-spacing: 0;
}
fieldset,img {
    border: 0;
}
address,caption,cite,code,dfn,em,strong,th,var,optgroup {
    font-style: inherit;
    font-weight: inherit;
}
del,ins {
    text-decoration: none;
}
li {
    list-style: disc;
    margin-left: 20px;
}
caption,th {
    text-align: left;
}
h1,h2,h3,h4,h5,h6 {
    font-size: 100%;
    font-weight: bold;
}
q:before,q:after {
    content: '';
}
abbr,acronym {
    border: 0;
    font-variant: normal;
}
sup {
    vertical-align: baseline;
}
sub {
    vertical-align: baseline;
}
legend {
    color: #000;
}
input,button,textarea,select,optgroup,option {
    font-family: inherit;
    font-size: inherit;
    font-style: inherit;
    font-weight: inherit;
}
input,button,textarea,select {*font-size:100%;
}
/* END RESET */

body {
    margin-left: 1em;
    margin-right: 1em;
    font-family: arial, helvetica, geneva, sans-serif;
    background-color: #ffffff; margin: 0px;
}

code, tt { font-family: monospace; }
span.parameter { font-family:monospace; }
span.parameter:after { content:"":""; }
span.types:before { content:""(""; }
span.types:after { content:"")""; }
.type { font-weight: bold; font-style:italic }

body, p, td, th { font-size: .95em; line-height: 1.2em;}

p, ul { margin: 10px 0 0 0px;}

strong { font-weight: bold;}

em { font-style: italic;}

h1 {
    font-size: 1.5em;
    margin: 0 0 20px 0;
}
h2, h3, h4 { margin: 15px 0 10px 0; }
h2 { font-size: 1.25em; }
h3 { font-size: 1.15em; }
h4 { font-size: 1.06em; }

a:link { font-weight: bold; color: #004080; text-decoration: none; }
a:visited { font-weight: bold; color: #006699; text-decoration: none; }
a:link:hover { text-decoration: underline; }

hr {
    color:#cccccc;
    background: #00007f;
    height: 1px;
}

blockquote { margin-left: 3em; }

ul { list-style-type: disc; }

p.name {
    font-family: ""Andale Mono"", monospace;
    padding-top: 1em;
}

pre.example {
    background-color: rgb(245, 245, 245);
    border: 1px solid silver;
    padding: 10px;
    margin: 10px 0 10px 0;
    font-family: ""Andale Mono"", monospace;
    font-size: .85em;
}

pre {
    background-color: rgb(245, 245, 245);
    border: 1px solid silver;
    padding: 10px;
    margin: 10px 0 10px 0;
    overflow: auto;
    font-family: ""Andale Mono"", monospace;
}


table.index { border: 1px #00007f; }
table.index td { text-align: left; vertical-align: top; }

#container {
    margin-left: 1em;
    margin-right: 1em;
    background-color: #f0f0f0;
}

#product {
    text-align: center;
    border-bottom: 1px solid #cccccc;
    background-color: #ffffff;
}

#product big {
    font-size: 2em;
}

#main {
    background-color: #f0f0f0;
    border-left: 2px solid #cccccc;
}

#navigation {
    float: left;
    width: 18em;
    vertical-align: top;
    background-color: #f0f0f0;
    overflow: visible;
}

#navigation h2 {
    background-color:#e7e7e7;
    font-size:1.1em;
    color:#000000;
    text-align: left;
    padding:0.2em;
    border-top:1px solid #dddddd;
    border-bottom:1px solid #dddddd;
}

#navigation ul
{
    font-size:1em;
    list-style-type: none;
    margin: 1px 1px 10px 1px;
}

#navigation li {
    text-indent: -1em;
    display: block;
    margin: 3px 0px 0px 22px;
}

#navigation li li a {
    margin: 0px 3px 0px -1em;
}

#content {
    margin-left: 18em;
    padding: 1em;
    width: 700px;
    border-left: 2px solid #cccccc;
    border-right: 2px solid #cccccc;
    background-color: #ffffff;
}

#about {
    clear: both;
    padding: 5px;
    border-top: 2px solid #cccccc;
    background-color: #ffffff;
}

@media print {
    body {
        font: 12pt ""Times New Roman"", ""TimeNR"", Times, serif;
    }
    a { font-weight: bold; color: #004080; text-decoration: underline; }

    #main {
        background-color: #ffffff;
        border-left: 0px;
    }

    #container {
        margin-left: 2%;
        margin-right: 2%;
        background-color: #ffffff;
    }

    #content {
        padding: 1em;
        background-color: #ffffff;
    }

    #navigation {
        display: none;
    }
    pre.example {
        font-family: ""Andale Mono"", monospace;
        font-size: 10pt;
        page-break-inside: avoid;
    }
}

table.module_list {
    border-width: 1px;
    border-style: solid;
    border-color: #cccccc;
    border-collapse: collapse;
}
table.module_list td {
    border-width: 1px;
    padding: 3px;
    border-style: solid;
    border-color: #cccccc;
}
table.module_list td.name { background-color: #f0f0f0; min-width: 200px; }
table.module_list td.summary { width: 100%; }


table.function_list {
    border-width: 1px;
    border-style: solid;
    border-color: #cccccc;
    border-collapse: collapse;
}
table.function_list td {
    border-width: 1px;
    padding: 3px;
    border-style: solid;
    border-color: #cccccc;
}
table.function_list td.name { background-color: #f0f0f0; min-width: 200px; }
table.function_list td.summary { width: 100%; }

ul.nowrap {
    overflow:auto;
    white-space:nowrap;
}

dl.table dt, dl.function dt {border-top: 1px solid #ccc; padding-top: 1em;}
dl.table dd, dl.function dd {padding-bottom: 1em; margin: 10px 0 0 20px;}
dl.table h3, dl.function h3 {font-size: .95em;}

/* stop sublists from having initial vertical space */
ul ul { margin-top: 0px; }
ol ul { margin-top: 0px; }
ol ol { margin-top: 0px; }
ul ol { margin-top: 0px; }

/* styles for prettification of source */
pre .comment { color: #558817; }
pre .constant { color: #a8660d; }
pre .escape { color: #844631; }
pre .keyword { color: #2239a8; font-weight: bold; }
pre .library { color: #0e7c6b; }
pre .marker { color: #512b1e; background: #fedc56; font-weight: bold; }
pre .string { color: #a8660d; }
pre .number { color: #f8660d; }
pre .operator { color: #2239a8; font-weight: bold; }
pre .preprocessor, pre .prepro { color: #a33243; }
pre .global { color: #800080; }
pre .prompt { color: #558817; }
pre .url { color: #272fc2; text-decoration: underline; }
";

	#endregion

	#region class template

	public static readonly string classTemplate = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN""
        ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">
<html>
<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/>
<head>
    <title>{0}</title>
    <link rel=""stylesheet"" href=""../css.css"" type=""text/css""/>
</head>
<body>

<div id=""container"">

    <div id=""product"">
        <div id=""product_logo""></div>
        <div id=""product_name""><big><b></b></big></div>
        <div id=""product_description""></div>
    </div> <!-- id=""product"" -->


    <div id=""main"">


        <!-- Menu -->

        <div id=""navigation"">
            <br/>
            <h1>{1}</h1>

            <ul>
                <li><a href=""../index.html"">Index</a></li>
            </ul>

            <h2>Contents</h2>
            <ul>
                <li><a href=""#Functions"">Functions</a></li>
            </ul>


            <h2>Classes</h2>
            <ul class=""$(kind=='Topics' and '' or 'nowrap'"">
{2}
            </ul>
            <h2>ReadMe</h2>
            <ul class=""$(kind=='Topics' and '' or 'nowrap'"">
                <li><a href=""../Readme.md.html"">Readme</a></li>
            </ul>

        </div>

        <div id=""content"">

            <h1>Class <code>{3}</code></h1>
            <p>{4}</p>
            <p>


            </p>
            <h3>Info:</h3>
            <ul>
                <li><strong>Copyright</strong>: 2017</li>
                <li><strong>Author</strong>: Albert Feng (SES XueLan)</li>
            </ul>


            <h2><a href=""#"">Properties</a></h2>
            <table class=""function_list"">
{5}
            </table>

            <br/>
            <br/>

            <h2><a href=""#Functions"">Functions</a></h2>
            <table class=""function_list"">
{6}
            </table>

            <br/>
            <br/>


            <h2><a name=""Functions""></a>Functions</h2>
            <dl class=""function"">
{7}
            </dl>


        </div> <!-- id=""content"" -->
    </div> <!-- id=""main"" -->
    <div id=""about"">
        <i>written by <a href=""https://github.com/SES-xuelan"">XueLan</a></i>
    </div> <!-- id=""about"" -->
</div> <!-- id=""container"" -->
</body>
</html>
";

	#endregion


}
