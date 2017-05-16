package example.frogs.connected.gpsnote;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

public class NotesAdapter extends BaseAdapter {
    private Context mContext;
    private LayoutInflater mInflater;
    private Note[] mDataSource;

    public NotesAdapter(Context context, Note[] items) {
        mContext = context;
        mDataSource = items;
        mInflater = (LayoutInflater) mContext.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
    }

    //1
    @Override
    public int getCount() {
        return mDataSource.length;
    }

    //2
    @Override
    public Object getItem(int position) {
        return mDataSource[position];
    }

    //3
    @Override
    public long getItemId(int position) {
        return position;
    }

    //4
    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        // Get view for row item
        View rowView = mInflater.inflate(R.layout.list_item_note, parent, false);

        // Get title element
        TextView titleTextView =
                (TextView) rowView.findViewById(R.id.note_list_text);

        Note note = (Note) getItem(position);

        titleTextView.setText(note.description);

        // Get title element
        TextView userNameTextView =
                (TextView) rowView.findViewById(R.id.note_list_user_name);

        userNameTextView.setText(note.user_name);

        return rowView;
    }
}
