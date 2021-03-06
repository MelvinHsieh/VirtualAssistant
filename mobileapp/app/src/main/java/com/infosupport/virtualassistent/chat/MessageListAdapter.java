package com.infosupport.virtualassistent.chat;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.infosupport.virtualassistent.R;
import com.infosupport.virtualassistent.model.Message;
import com.squareup.picasso.Picasso;

import java.text.DateFormat;
import java.util.List;

public class MessageListAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder> {
    private static final int VIEW_TYPE_MESSAGE_SENT = 1;
    private static final int VIEW_TYPE_MESSAGE_RECEIVED = 2;
    private static final int VIEW_TYPE_IMAGE_RECEIVED = 3;

    private List<Message> mMessageList;

    public MessageListAdapter(List<Message> messageList) {
        mMessageList = messageList;
    }

    @Override
    public int getItemCount() {
        return mMessageList.size();
    }

    // Determines the appropriate ViewType according to the sender of the message.
    @Override
    public int getItemViewType(int position) {
        Message message = mMessageList.get(position);

        if (message.isUser) {
            // If the current user is the sender of the message
            return VIEW_TYPE_MESSAGE_SENT;
        } else {
            // If some other user sent the message
            if(message.isImage) {
                // If the message contains an image url
                return VIEW_TYPE_IMAGE_RECEIVED;
            }
            return VIEW_TYPE_MESSAGE_RECEIVED;
        }
    }

    // Inflates the appropriate layout according to the ViewType.
    @NonNull
    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view;

        if (viewType == VIEW_TYPE_MESSAGE_SENT) {
            view = LayoutInflater.from(parent.getContext())
                    .inflate(R.layout.frame_chat_sent, parent, false);
            return new SentMessageHolder(view);
        } else if (viewType == VIEW_TYPE_IMAGE_RECEIVED) {
            view = LayoutInflater.from(parent.getContext())
                    .inflate(R.layout.frame_image_received, parent, false);
            return new ReceivedImageHolder(view);
        } else {
            // If the chat wasn't sent, it can be assumed it's a received message.
            view = LayoutInflater.from(parent.getContext())
                    .inflate(R.layout.frame_chat_received, parent, false);
            return new ReceivedMessageHolder(view);
        }
    }

    // Passes the message object to a ViewHolder so that the contents can be bound to UI.
    @Override
    public void onBindViewHolder(RecyclerView.ViewHolder holder, int position) {
        Message message = mMessageList.get(position);

        switch (holder.getItemViewType()) {
            case VIEW_TYPE_MESSAGE_SENT:
                ((SentMessageHolder) holder).bind(message);
                break;
            case VIEW_TYPE_MESSAGE_RECEIVED:
                ((ReceivedMessageHolder) holder).bind(message);
                break;
            case VIEW_TYPE_IMAGE_RECEIVED:
                ((ReceivedImageHolder) holder).bind(message);
        }
    }

    private static class SentMessageHolder extends RecyclerView.ViewHolder {
        TextView messageText, timeText;

        SentMessageHolder(View itemView) {
            super(itemView);

            messageText = (TextView) itemView.findViewById(R.id.text_message_body_me);
            timeText = (TextView) itemView.findViewById(R.id.text_message_time_me);
        }

        void bind(Message message) {
            messageText.setText(message.message);

            // Format the stored timestamp into a readable String using method.
            timeText.setText(DateFormat.getDateInstance().format(message.createdAt));
        }
    }

    private static class ReceivedMessageHolder extends RecyclerView.ViewHolder {
        TextView messageText, timeText, nameText;

        ReceivedMessageHolder(View itemView) {
            super(itemView);

            messageText = (TextView) itemView.findViewById(R.id.text_message_body_other);
            timeText = (TextView) itemView.findViewById(R.id.text_message_time_other);
            nameText = (TextView) itemView.findViewById(R.id.text_user_other);
        }

        void bind(Message message) {
            messageText.setText(message.message);

            // Format the stored timestamp into a readable String using method.
            timeText.setText(DateFormat.getDateInstance().format(message.createdAt));

            nameText.setText(R.string.assistant_name);
        }
    }

    private static class ReceivedImageHolder extends RecyclerView.ViewHolder {
        TextView timeText, nameText;
        ImageView image;

        ReceivedImageHolder(View itemView) {
            super(itemView);

            image = (ImageView) itemView.findViewById(R.id.text_message_image_other);
            timeText = (TextView) itemView.findViewById(R.id.text_message_time_other);
            nameText = (TextView) itemView.findViewById(R.id.text_user_other);
        }

        void bind(Message message) {
            String image_url = message.message;
            if(image_url.isEmpty()) {
                image_url = "Image not found";
            }
            Picasso.get()
                    .load(image_url)
                    .placeholder(R.drawable.image_not_found)
                    .resize(750, 750)
                    .into(image);

            // Format the stored timestamp into a readable String using method.
            timeText.setText(DateFormat.getDateInstance().format(message.createdAt));

            nameText.setText(R.string.assistant_name);
        }
    }

}