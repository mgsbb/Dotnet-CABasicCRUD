type Props = {
  label: string;
  id: string;
  type: string;
  value: string | number;
  onChange:
    | React.ChangeEventHandler<HTMLInputElement, HTMLInputElement>
    | undefined;
};

export default function AuthInput({ id, type, label, value, onChange }: Props) {
  return (
    <label
      htmlFor={id}
      className="font-medium text-gray-500 flex flex-col gap-1"
    >
      {label}
      <input
        type={type}
        id={id}
        name={id}
        value={value}
        onChange={onChange}
        className="border border-gray-300 rounded-sm p-2 text-black font-normal"
      />
    </label>
  );
}
