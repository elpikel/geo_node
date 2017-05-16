package example.frogs.connected.gpsnote;

import android.os.AsyncTask;

import com.google.gson.Gson;
import com.squareup.okhttp.Call;
import com.squareup.okhttp.MediaType;
import com.squareup.okhttp.OkHttpClient;
import com.squareup.okhttp.Request;
import com.squareup.okhttp.RequestBody;
import com.squareup.okhttp.Response;

import java.io.IOException;

// used when we do not have any note in given place
public class SaveNoteWithoutPlace extends AsyncTask<Void, Note, Note> {
    public final MediaType JSON = MediaType.parse("application/json; charset=utf-8");

    private NoteWithoutPlace note;

    public SaveNoteWithoutPlace(NoteWithoutPlace note) {
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
        // bind notes to view
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
            return RequestBody.create(JSON, gson.toJson(new NoteNoteWithoutPlacePostBody(this.note)));
        } catch (Exception e) {
            throw new IOException("problem creating json from note");
        }
    }

    private Note convert(Response response) throws IOException {
        String jsonMessages = response.body().string();

        return new Gson().fromJson(jsonMessages, Note.class);
    }
}
