export type TConversation = {
  id: string;
  conversationType: "private" | "group";
  participants: TParticipant[];
  messages: TMessage[];
  createdAt: string;
  updatedAt: string;
};

export type TParticipant = {
  participantUserId: string;
  participantUsername: string;
  participantFullName: string;
};

export type TMessage = {
  id: string;
  content: string;
  senderUserId: string;
  senderUsername: string;
  senderFullName: string;
  createdAt: string;
  updatedAt?: string;
};

export type TMessageFromHub = {
  messageId: string;
  content: string;
  senderUserId: string;
  senderUsername: string;
  senderFullName: string;
  sentAt: string;
  conversationId: string;
};
