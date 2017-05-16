package example.frogs.connected.gpsnote;

import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ListView;

import com.google.gson.Gson;
import com.google.gson.JsonParseException;
import com.squareup.okhttp.Call;
import com.squareup.okhttp.MediaType;
import com.squareup.okhttp.OkHttpClient;
import com.squareup.okhttp.Request;
import com.squareup.okhttp.RequestBody;
import com.squareup.okhttp.Response;

import java.io.IOException;

public class NotesActivity extends AppCompatActivity {

    private ListView mListView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_notes);

        mListView = (ListView) findViewById(R.id.notes_list_view);

        Bundle b = getIntent().getExtras();
        int placeId = b.getInt("placeId");

        Button button = (Button) this.findViewById(R.id.add_note_to_place);

        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // save note
                Note note = new Note();
                //note.description =
                new SaveNote(note).execute();
            }
        });

        new GetNotes(placeId).execute();
    }

    private class SaveNote extends AsyncTask<Void, Note, Note> {
        public final MediaType JSON = MediaType.parse("application/json; charset=utf-8");

        private Note note;

        public SaveNote(Note note) {
            this.note = note;
        }

        @Override
        protected Note doInBackground(Void... params) {
            try {
                Call registerCall = createCall();

                Response response = registerCall.execute();

                return convert(response);

            } catch (IOException e) {
                e.printStackTrace();
            }

            return null;
        }

        protected void onPostExecute(Note note) {
            // clear form
            new GetNotes(note.place_id).execute();
        }

        private Call createCall() throws IOException {
            OkHttpClient client = new OkHttpClient();

            Request request = new Request.Builder()
                    .url("https://young-thicket-35712.herokuapp.com/notes")
                    .post(createRequestBody())
                    .build();

            return client.newCall(request);
        }

        private RequestBody createRequestBody() throws IOException {
            Gson gson = new Gson();
            try {
                return RequestBody.create(JSON, gson.toJson(new NotePostBody(this.note)));
            } catch (Exception e) {
                throw new IOException("problem creating json from note");
            }
        }

        private Note convert(Response response) throws IOException {
            String jsonMessages = response.body().string();

            return new Gson().fromJson(jsonMessages, Note.class);
        }
    }

    private class GetNotes extends AsyncTask<Void, Void, Note[]> {

        private int placeId;

        public GetNotes(int placeId) {

            this.placeId = placeId;
        }

        private Call createCall() {
            OkHttpClient client = new OkHttpClient();

            Request request = new Request.Builder()
                    .url("https://young-thicket-35712.herokuapp.com/notes?place_id="+placeId)
                    .build();

            return client.newCall(request);
        }

        private Note[] convert(Response response) throws IOException {
            try {
                return new Gson().fromJson(response.body().string(), Note[].class);
            } catch (JsonParseException e) {
                return new Note[0];
            }
        }

        protected Note[] doInBackground(Void... nothing) {

            Call call = createCall();

            try {
                Response response = call.execute();

                if(response.code() == 400) {
                    return new Note[0];
                }

                return convert(response);
            } catch (IOException e) {
                e.printStackTrace();
            }

            return new Note[0];
        }

        protected void onPostExecute(Note[] notes) {
            NotesAdapter adapter = new NotesAdapter(NotesActivity.this, notes);
            mListView.setAdapter(adapter);
        }
    }
}
