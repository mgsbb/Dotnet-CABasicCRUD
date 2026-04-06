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
import { queryClient } from "../../../App";

// ===================================================================================================================
// ===================================================================================================================

export default function Logout() {
  const [isOpen, setIsOpen] = useState(false);

  const navigate = useNavigate();

  // -------------------------------------------------------------------------------------------------------------------

  const logoutMutation = useMutation({
    mutationFn: async () => {
      const response = await axios.post(
        "/api/v1/auth/logout",
        {},
        { withCredentials: true },
      );
      return response.data;
    },

    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["auth"] });
      navigate("/auth/login");
    },

    onError: (error: any) => {
      console.error(error);
    },
  });

  // -------------------------------------------------------------------------------------------------------------------

  const handleLogoutSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    logoutMutation.mutate();

    setIsOpen(false);
  };

  // -------------------------------------------------------------------------------------------------------------------

  return (
    <>
      <div className="inline-flex gap-4 cursor-pointer">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          strokeWidth="1.5"
          stroke="currentColor"
          className="size-6"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            d="M15.75 9V5.25A2.25 2.25 0 0 0 13.5 3h-6a2.25 2.25 0 0 0-2.25 2.25v13.5A2.25 2.25 0 0 0 7.5 21h6a2.25 2.25 0 0 0 2.25-2.25V15M12 9l-3 3m0 0 3 3m-3-3h12.75"
          />
        </svg>

        <button
          type="button"
          onClick={() => setIsOpen(true)}
          className="cursor-pointer sidebar-label"
        >
          Logout
        </button>
      </div>

      <Dialog
        open={isOpen}
        onClose={() => setIsOpen(false)}
        className="relative z-50"
      >
        <div className="fixed inset-0 bg-black/40" aria-hidden="true" />

        <div className="fixed inset-0 flex w-screen items-center justify-center p-4">
          <DialogPanel className="max-w-lg space-y-4 border border-gray-200 rounded-md bg-white p-6 sm:p-12">
            <DialogTitle className="font-bold">Logout</DialogTitle>
            <Description>Are you sure?</Description>
            <div className="flex gap-4">
              {/* logout */}
              <form
                onSubmit={handleLogoutSubmit}
                className="inline-flex gap-4 cursor-pointer"
              >
                <button
                  type="submit"
                  className="bg-gray-200 px-2 py-1 text-gray-700 font-semibold rounded-sm cursor-pointer"
                >
                  Logout
                </button>
                <button
                  type="button"
                  onClick={() => setIsOpen(false)}
                  className="px-2 py-1 text-gray-700 font-semibold rounded-sm cursor-pointer"
                >
                  Cancel
                </button>
              </form>
            </div>
          </DialogPanel>
        </div>
      </Dialog>
    </>
  );
}
