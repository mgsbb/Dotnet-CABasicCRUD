import { useMutation } from "@tanstack/react-query";
import axios, { AxiosError } from "axios";
import { useNavigate } from "react-router";

// ===================================================================================================================
// ===================================================================================================================

type Props = {
  currentUserId: string;
  targetUserId: string;
};

type TConversation = {
  id: string;
  participantsId: string[];
};

// ===================================================================================================================
// ===================================================================================================================

export default function StartPrivateConversation({
  currentUserId,
  targetUserId,
}: Props) {
  const navigate = useNavigate();

  // -------------------------------------------------------------------------------------------------------------------

  const startPrivateConversationMutation = useMutation({
    mutationFn: async (): Promise<TConversation> => {
      const response = await axios.post(
        `/api/v1/conversations`,
        {
          conversationType: "private",
          groupTitle: null,
          participantUserIds: [currentUserId, targetUserId],
        },
        { withCredentials: true },
      );
      return response.data;
    },

    onSuccess: (data) => {
      navigate(`/conversations/${data.id}`);
    },

    onError: (err: AxiosError) => {
      console.error(err);
    },
  });

  // -------------------------------------------------------------------------------------------------------------------

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    startPrivateConversationMutation.mutate();
  };

  // -------------------------------------------------------------------------------------------------------------------

  return (
    <div className="">
      <form onSubmit={handleSubmit} className="bg-blue-100 items-start">
        <input type="hidden" name="targetUserId" value="@Model.Id" />
        <button
          type="submit"
          className="px-4 py-2 text-white bg-gray-900 rounded-sm cursor-pointer text-sm "
        >
          Message
        </button>
      </form>
    </div>
  );
}
