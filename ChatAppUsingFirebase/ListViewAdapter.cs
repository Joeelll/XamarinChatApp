using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System.Collections.Generic;

namespace ChatAppUsingFirebase
{
    internal class ListViewAdapter : BaseAdapter
    {
        private MainActivity mainActivity;
        private List<MessageContent> lstMessage;

        public ListViewAdapter(MainActivity mainActivity, List<MessageContent> lstMessage)
        {
            this.mainActivity = mainActivity;
            this.lstMessage = lstMessage;
        }

        public override int Count
        {
            get { return lstMessage.Count; }
        }

        public override Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)mainActivity.BaseContext.GetSystemService(Context.LayoutInflaterService);
            View ItemView = inflater.Inflate(Resource.Layout.List_Item, null);
            TextView message_user, message_time, message_content;
            message_user = ItemView.FindViewById<TextView>(Resource.Id.message_user);
            message_time = ItemView.FindViewById<TextView>(Resource.Id.message_time);
            message_content = ItemView.FindViewById<TextView>(Resource.Id.message_text);

            message_user.Text = lstMessage[position].Email;
            message_time.Text = lstMessage[position].Time;
            message_content.Text = lstMessage[position].Message;

            return ItemView;
        }
    }
}