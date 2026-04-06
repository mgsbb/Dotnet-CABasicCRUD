import {
  Description,
  Dialog,
  DialogPanel,
  DialogTitle,
} from "@headlessui/react";
import { useMutation } from "@tanstack/react-query";
import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router";

// ===================================================================================================================
// ===================================================================================================================

export default function DeletePost({ postId }: { postId?: string }) {
  const [isOpen, setIsOpen] = useState(false);

  const navigate = useNavigate();

  // -------------------------------------------------------------------------------------------------------------------

  const deletePostMutation = useMutation({
    mutationFn: async () => {
      const response = await axios.delete(`/api/v1/posts/${postId}`, {
        withCredentials: true,
      });

      return response.data;
    },

    onSuccess: () => {
      navigate(`/posts`);
    },

    onError: (err: any) => {
      console.error(err);
    },
  });

  // -------------------------------------------------------------------------------------------------------------------

  if (postId === null) {
    return null;
  }

  // -------------------------------------------------------------------------------------------------------------------

  return (
    <>
      <button
        type="button"
        onClick={() => setIsOpen(true)}
        className="bg-red-100 px-2 py-1 text-red-700 font-semibold rounded-sm cursor-pointer"
      >
        Delete
      </button>

      <Dialog
        open={isOpen}
        onClose={() => setIsOpen(false)}
        className="relative z-50"
      >
        <div className="fixed inset-0 bg-black/40" aria-hidden="true" />

        <div className="fixed inset-0 flex w-screen items-center justify-center p-4">
          <DialogPanel className="max-w-lg space-y-4 border border-gray-200 rounded-md bg-white p-6 sm:p-12">
            <DialogTitle className="font-bold">Delete Post</DialogTitle>
            <Description>Are you sure?</Description>
            <p>This action is not reversible.</p>
            <div className="flex gap-4">
              <button
                onClick={() => {
                  deletePostMutation.mutate();
                  setIsOpen(false);
                }}
                className="bg-red-100 px-2 py-1 text-red-700 font-semibold rounded-sm cursor-pointer"
              >
                Delete
              </button>
              <button
                onClick={() => setIsOpen(false)}
                className="bg-gray-100 px-2 py-1 text-gray-700 font-semibold rounded-sm cursor-pointer"
              >
                Cancel
              </button>
            </div>
          </DialogPanel>
        </div>
      </Dialog>
    </>
  );
}
