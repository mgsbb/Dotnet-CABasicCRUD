import { useMutation, useQuery } from "@tanstack/react-query";
import { useAuth } from "../../hooks/useAuth";
import { Link, useParams } from "react-router";
import axios, { AxiosError } from "axios";
import { useEffect, useState } from "react";
import { createConnection } from "../../services/signalr";

type TConversation = {
  id: string;
  conversationType: "private" | "group";
  participants: TParticipant[];
  messages: TMessage[];
  createdAt: string;
  updatedAt: string;
};

type TParticipant = {
  participantUserId: string;
  participantUsername: string;
  participantFullName: string;
};

type TMessage = {
  id: string;
  content: string;
  senderUserId: string;
  senderUsername: string;
  senderFullName: string;
  createdAt: string;
  updatedAt?: string;
};

type TMessageFromHub = {
  messageId: string;
  content: string;
  senderUserId: string;
  senderUsername: string;
  senderFullName: string;
  sentAt: string;
  conversationId: string;
};

function formatTime(timestamp: string): string {
  console.log(timestamp);
  const date = timestamp.split("T")[0];
  const time = timestamp.split("T")[1].split(".")[0];
  return `${date} ${time}`;
}

export default function PrivateConversation() {
  const { id: conversationId } = useParams();

  const { data: currentUser } = useAuth();

  const [message, setMessage] = useState("");

  const {
    data: conversation,
    isLoading,
    isError,
  } = useQuery({
    queryKey: ["", conversationId],
    queryFn: async (): Promise<TConversation> => {
      const response = await axios.get(
        `/api/v1/conversations/${conversationId}`,
        { withCredentials: true },
      );
      return response.data;
    },
  });

  const [messages, setMessages] = useState<TMessage[]>([]);

  useEffect(() => {
    console.log("running");
    if (conversation) {
      setMessages(conversation.messages);
    }
  }, [conversation?.id]);

  useEffect(() => {
    const connection = createConnection("/api/v1/hubs/chat");

    connection
      .start()
      .then(() => {
        console.log("Connected");
        return connection.invoke("JoinConversation", conversationId);
      })
      .then(() => console.log("Joined group"))
      .catch((err) => console.error(err));

    connection.on("MessageReceived", function (message: TMessageFromHub) {
      const newMessage: TMessage = {
        id: message.messageId,
        content: message.content,
        senderUserId: message.senderUserId,
        senderUsername: message.senderUsername,
        senderFullName: message.senderFullName,
        createdAt: message.sentAt,
        updatedAt: undefined,
      };

      setMessages((prev) => {
        const newState = [...prev, newMessage];
        // console.log(newState);
        return newState;
      });
    });

    return () => {
      connection.stop();
    };
  }, []);

  const sendMessageMutation = useMutation({
    mutationFn: async (message: string) => {
      const response = await axios.post(
        `/api/v1/conversations/${conversationId}/messages`,
        { content: message },
        { withCredentials: true },
      );
      return response.data;
    },

    onError: (err: AxiosError) => {
      console.error(err);
    },

    onSuccess: () => {},
  });

  const handleSendMessage = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    sendMessageMutation.mutate(message);

    setMessage("");
  };

  if (isLoading) return <>Loading...</>;

  if (isError) return <>Error...</>;

  const otherUser = conversation?.participants.filter(
    (p) => p.participantUserId !== currentUser?.id,
  )[0];

  return (
    <div className="mx-auto bg-white h-screen flex flex-col gap-2">
      {/* header - display target username  */}
      <section className="w-full border-b border-gray-300 px-4 sm:px-10 flex gap-6 items-center py-2">
        <div className="flex gap-6 items-end">
          {/*  replace with user image */}
          <div className="w-12 h-12 border border-gray-400 rounded-xs"></div>
          <Link to={`/users/${otherUser?.participantUserId}`}>
            <p className="text-gray-700 font-medium text-xl">
              {otherUser?.participantFullName}
            </p>
          </Link>
        </div>
      </section>

      {/* <pre>{JSON.stringify(messages, null, 2)}</pre> */}

      {/* {messages.map((m) => (
        <div key={m.id}>{JSON.stringify(m)}</div>
      ))} */}

      {/*  messages  */}
      <section className="px-4 sm:px-10 overflow-y-auto flex-1">
        {messages.length === 0 && (
          <p className="text-gray-600 text-center ">Send your first message</p>
        )}
        {messages.length !== 0 && (
          <div id="messages-container" className="flex flex-col gap-6">
            {messages.map((message) => {
              const isCurrentUser = message.senderUserId === currentUser?.id;
              // console.log(message.id);
              return (
                <div
                  key={message.id}
                  className={`flex ${isCurrentUser ? "justify-end" : "justify-start"}`}
                >
                  <div
                    className={`${isCurrentUser ? "bg-purple-200" : "bg-gray-200"} flex gap-2 w-fit max-w-xs wrap-break-word flex-col py-2 px-6 rounded-sm `}
                  >
                    <p
                      className={`${isCurrentUser ? "text-purple-600" : "text-gray-600"} font-medium`}
                    >
                      {message.content}
                    </p>
                    <div
                      className={`flex gap-2 items-center ${isCurrentUser ? "justify-end" : "justify-start"}`}
                    >
                      <p
                        className={`text-sm font-semibold  ${isCurrentUser ? "text-purple-900" : "text-gray-900"}`}
                      >
                        {message.senderFullName}
                      </p>
                      <small
                        className={`${isCurrentUser ? "text-purple-500" : "text-gray-500"} text-xs font-medium`}
                      >
                        at {formatTime(message.createdAt)}
                      </small>
                    </div>
                  </div>
                </div>
              );
            })}
          </div>
        )}
      </section>
      {/* @* send message form *@ */}
      <form
        id="sendMessageForm"
        onSubmit={handleSendMessage}
        className="border-t border-gray-300 w-full flex py-2 sm:py-4 px-2 sm:px-10 justify-between gap-2 sm:gap-6 items-center"
      >
        <div className="flex flex-col gap-1 w-full">
          <textarea
            value={message}
            onChange={(e) => {
              setMessage(e.target.value);
            }}
            className=" border border-gray-300 rounded-sm p-2"
            rows={1}
          ></textarea>
        </div>
        <button
          type="submit"
          className="px-4 py-2 bg-gray-900 text-white rounded-sm cursor-pointer text-sm"
        >
          Send
        </button>
      </form>
    </div>
  );
}
