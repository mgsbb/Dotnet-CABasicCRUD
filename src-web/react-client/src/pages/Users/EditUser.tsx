import EditUserEmail from "./EditUserEmail";
import EditUserProfile from "./EditUserProfile";

export default function EditUser() {
  return (
    <>
      <section className="px-2 sm:px-10 flex flex-col gap-20 pb-20">
        <h1 className="text-3xl font-medium text-gray-700 text-center">
          Edit User
        </h1>

        <EditUserProfile />

        <EditUserEmail />
      </section>
    </>
  );
}
