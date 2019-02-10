# FAWinFormsLogin

This project contains a standalone version of [Furloader's](https://github.com/Kycklingar/Furloader) FurAffinity login window.

![](https://i.imgur.com/o7McH6l.png)

Example usage (C#):

	string b = null, a = null;
    using (var f = new LoginFormFA()) {
        if (f.ShowDialog() == DialogResult.OK) {
            b = f.BCookie;
			a = f.ACookie;
        }
    }
	// pass a and b to your favorite library or program
