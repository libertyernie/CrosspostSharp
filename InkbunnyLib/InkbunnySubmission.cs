using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkbunnyLib {
    public abstract class InkbunnySubmission {
        public int submission_id;
        public string hidden;
        public string username;
        public int user_id;
        public string create_datetime;
        public string create_datetime_usertime;
        public string last_file_update_datetime;
        public string last_file_update_datetime_usertime;
        public string file_name;
        public string thumbnail_url_medium;
        public string thumbnail_url_large;
        public string thumbnail_url_huge;
        public string thumbnail_url_medium_noncustom;
        public string thumbnail_url_large_noncustom;
        public string thumbnail_url_huge_noncustom;
        public string latest_file_name;
        public string latest_thumbnail_url_medium;
        public string latest_thumbnail_url_large;
        public string latest_thumbnail_url_huge;
        public string latest_thumbnail_url_medium_noncustom;
        public string latest_thumbnail_url_large_noncustom;
        public string latest_thumbnail_url_huge_noncustom;
        public string title;
        public string deleted;
        public string @public;
        public string mimetype;
        public int pagecount;
        public string latest_mimetype;
        public int rating_id;
        public string rating_name;
        public SubmissionType submission_type_id;
        public string type_name;
        public string digitalsales;
        public string printsales;
        public string friends_only;
        public string guest_block;
        public string scraps;
    }

	public class InkbunnySearchSubmission : InkbunnySubmission {
		public string file_url_full;
		public string file_url_screen;
		public string file_url_preview;
	}

	public class InkbunnySubmissionDetail : InkbunnySubmission {
		public IEnumerable<InkbunnyFile> files;
		public string description;
		public string description_bbcode_parsed;
		public string writing;
		public string writing_bbcode_parsed;
	}

	public class InkbunnyFile {
		public int file_id;
		public string file_name;
		public string thumbnail_url_medium;
		public string thumbnail_url_large;
		public string thumbnail_url_huge;
		public string thumbnail_url_medium_noncustom;
		public string thumbnail_url_large_noncustom;
		public string thumbnail_url_huge_noncustom;
		public string file_url_full;
		public string file_url_screen;
		public string file_url_preview;
		public string mimetype;
		public int submission_id;
		public int user_id;
		public int submission_file_order;
		public string initial_file_md5;
		public string full_file_md5;
		public string large_file_md5;
		public string small_file_md5;
		public string thumbnail_file_md5;
		public string deleted;
		public string create_datetime;
		public string create_datetime_usertime;
	}
}
