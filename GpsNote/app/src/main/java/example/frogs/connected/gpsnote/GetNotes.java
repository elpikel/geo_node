package example.frogs.connected.gpsnote;

import android.os.AsyncTask;

import com.google.gson.Gson;
import com.google.gson.JsonParseException;
import com.squareup.okhttp.Call;
import com.squareup.okhttp.OkHttpClient;
import com.squareup.okhttp.Request;
import com.squareup.okhttp.Response;

import java.io.IOException;

public class GetNotes extends AsyncTask<Void, Void, Note[]> {

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
        // bind notes to view
    }
}
