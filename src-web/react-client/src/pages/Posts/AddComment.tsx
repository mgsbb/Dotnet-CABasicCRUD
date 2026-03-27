import { useMutation } from "@tanstack/react-query";
import axios from "axios";
import { useState } from "react";
import { queryClient } from "../../App";

export default function AddComment({ postId }: { postId?: string }) {
  const [comment, setComment] = useState("");

  const createCommentMutation = useMutation({
    mutationFn: async (data: string) => {
      const response = await axios.post(
        `/api/v1/posts/${postId}/comments`,
        // must match the body shape required by the api
        { body: data },
        {
          withCredentials: true,
        },
      );

      return response.data;
    },

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["posts", postId, "comments"],
      });
    },

    onError: (err: any) => {
      console.error(err);
    },
  });

  const handleCommentSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    createCommentMutation.mutate(comment);
  };

  const handleChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    setComment(e.target.value);
  };

  return (
    <section className="mt-16 w-full">
      <h6 className="font-bold text-gray-700">Add a comment</h6>

      <form onSubmit={handleCommentSubmit} className="mt-5 flex flex-col gap-3">
        <div className="flex flex-col gap-1">
          <textarea
            id="comment"
            name="comment"
            value={comment}
            onChange={handleChange}
            placeholder="Write your comment..."
            className="border border-gray-300 rounded-sm p-2"
          ></textarea>
        </div>

        <button
          type="submit"
          className="bg-gray-900 text-white py-2 px-4 rounded-sm cursor-pointer font-semibold text-sm w-fit"
        >
          Submit
        </button>
      </form>
    </section>
  );
}
