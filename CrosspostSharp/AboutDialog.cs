using System.Windows.Forms;

namespace CrosspostSharp {
	public class AboutDialog : Form {
		public AboutDialog() {
            Width = 500;
            Height = 400;

            var webBrowser = new WebBrowser {
                Dock = DockStyle.Fill
            };
            Controls.Add(webBrowser);

            Load += (o, e) => {
                webBrowser.Navigate("about:blank");
                webBrowser.Document.Write(@"<html>
<head>
<meta http-equiv=""""content-type"""" content=""""text/html; charset=UTF-8"""">
<title>About</title>
</head>
<body>
<h1>CrosspostSharp 2.2</h1>
<p><a href=""""https://github.com/libertyernie/CrosspostSharp"""">https://github.com/libertyernie/CrosspostSharp</a><br>
</p>
<p>This product uses the Flickr API but is not endorsed or certified
by Flickr.<br>
</p>
<hr width=""""100%"""" size=""""2"""">Copyright © 2014-2017 libertyernie<br>
<p><i>WeasylLib</i> - Copyright © 2014-2017 libertyernie<br>
<i>InkbunnyLib</i><i> </i>- Copyright © 2017 libertyernie<br>
<i>FAExportLib</i><i> </i>- Copyright © 2017 libertyernie<br>
<i>Tumblr#</i> - Copyright © 2013 Don't Panic Software (<a
href=""""https://tumblrsharp.codeplex.com/"""">https://tumblrsharp.codeplex.com/</a>)<br>
<i>Html Agility Pack </i>- Copyright © 2014 - 2017 ZZZ Projects
Inc. (<a href=""""https://github.com/zzzprojects/html-agility-pack"""">https://github.com/zzzprojects/html-agility-pack)</a><br>
<i>Json.NET </i>- Copyright © 2007 James Newton-King (<a
href=""""http://www.newtonsoft.com/json"""">http://www.newtonsoft.com/json)</a><br>
<i>AsyncEx </i>- Copyright (c) 2014 StephenCleary (<a
href=""""https://github.com/StephenCleary/AsyncEx"""">https://github.com/StephenCleary/AsyncEx</a>)</p>
<p>Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the """"Software""""), to deal in the Software without
restriction, including without limitation the rights to use, copy,
modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:<br>
</p>
<p>The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.<br>
</p>
<p>THE SOFTWARE IS PROVIDED """"AS IS"""", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
DEALINGS IN THE SOFTWARE.<br>
</p>
<hr width=""""100%"""" size=""""2"""">
<p><i>FAWinFormsLogin </i>(<a
href=""""https://github.com/libertyernie/FAWinFormsLogin"""">https://github.com/libertyernie/FAWinFormsLogin</a>)</p>
<p>This is free and unencumbered software released into the public
domain.</p>
<p>Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a
compiled binary, for any purpose, commercial or non-commercial,
and by any means.<br>
</p>
<p>In jurisdictions that recognize copyright laws, the author or
authors of this software dedicate any and all copyright interest
in the software to the public domain. We make this dedication for
the benefit of the public at large and to the detriment of our
heirs and successors. We intend this dedication to be an overt act
of relinquishment in perpetuity of all present and future rights
to this software under copyright law.<br>
</p>
<p>THE SOFTWARE IS PROVIDED """"AS IS"""", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.<br>
</p>
<p>For more information, please refer to &lt;<a
href=""""http://unlicense.org"""">http://unlicense.org</a>&gt;<br>
</p>
<hr width=""""100%"""" size=""""2"""">
<p><i>DeviantartApi </i>(<a
href=""""https://github.com/Mr1Penguin/DeviantartApi"""">https://github.com/Mr1Penguin/DeviantartApi</a>)<br>
</p>
<p>No license information available<br>
</p>
<hr width=""""100%"""" size=""""2"""">
<p><i>FlickrNet </i>- Copyright © 2016 Sam Judson (<a
href=""""https://github.com/samjudson/flickr-net"""">https://github.com/samjudson/flickr-net</a>)<br>
<br>
Licensed under the Apache License, Version 2.0 (the """"License"""");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at:<br>
</p>
<p><a href=""""http://www.apache.org/licenses/LICENSE-2.0"""">http://www.apache.org/licenses/LICENSE-2.0</a></p>
<p>Unless required by applicable law or agreed to in writing,
software distributed under the License is distributed on an """"AS
IS"""" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
express or implied. See the License for the specific language
governing permissions and limitations under the License.</p>
<hr width=""""100%"""" size=""""2""""><i>Tweetinvi </i>(<a
href=""""https://github.com/linvi/tweetinvi"""">https://github.com/linvi/tweetinvi)</a><br>
<br>
This license governs use of the accompanying software. If you use
the software, you accept this license. If you do not accept the
license, do not use the software.<br>
<br>
1. Definitions<br>
The terms """"reproduce,"""" """"reproduction,"""" """"derivative works,"""" and
""""distribution"""" have the same meaning here as under U.S. copyright
law.<br>
A """"contribution"""" is the original software, or any additions or
changes to the software.<br>
A """"contributor"""" is any person that distributes its contribution
under this license.<br>
""""Licensed patents"""" are a contributor's patent claims that read
directly on its contribution.<br>
<br>
2. Grant of Rights<br>
(A) Copyright Grant- Subject to the terms of this license, including
the license conditions and limitations in section 3, each
contributor grants you a non-exclusive, worldwide, royalty-free
copyright license to reproduce its contribution, prepare derivative
works of its contribution, and distribute its contribution or any
derivative works that you create.<br>
(B) Patent Grant- Subject to the terms of this license, including
the license conditions and limitations in section 3, each
contributor grants you a non-exclusive, worldwide, royalty-free
license under its licensed patents to make, have made, use, sell,
offer for sale, import, and/or otherwise dispose of its contribution
in the software or derivative works of the contribution in the
software.<br>
<br>
3. Conditions and Limitations<br>
(A) No Trademark License- This license does not grant you rights to
use any contributors' name, logo, or trademarks.<br>
(B) If you bring a patent claim against any contributor over patents
that you claim are infringed by the software, your patent license
from such contributor to the software ends automatically.<br>
(C) If you distribute any portion of the software, you must retain
all copyright, patent, trademark, and attribution notices that are
present in the software.<br>
(D) If you distribute any portion of the software in source code
form, you may do so only under this license by including a complete
copy of this license with your distribution. If you distribute any
portion of the software in compiled or object code form, you may
only do so under a license that complies with this license.<br>
(E) The software is licensed """"as-is."""" You bear the risk of using it.
The contributors give no express warranties, guarantees or
conditions. You may have additional consumer rights under your local
laws which this license cannot change. To the extent permitted under
your local laws, the contributors exclude the implied warranties of
merchantability, fitness for a particular purpose and
non-infringement.<br>
<br>
<br>
</body>
</html>

");
            };
		}
    }
}
