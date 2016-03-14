#pragma once

namespace bassmidi_win32drv {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	/// <summary>
	/// Riepilogo per DebugInfo
	/// </summary>
	public ref class DebugInfo : public System::Windows::Forms::Form
	{
	public:
		DebugInfo(void)
		{
			InitializeComponent();
			//
			//TODO: aggiungere qui il codice del costruttore.
			//
		}

	protected:
		/// <summary>
		/// Pulire le risorse in uso.
		/// </summary>
		~DebugInfo()
		{
			if (components)
			{
				delete components;
			}
		}

	private:
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		void InitializeComponent(void)
		{
			this->SuspendLayout();
			// 
			// DebugInfo
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(284, 262);
			this->Name = L"DebugInfo";
			this->Text = L"DebugInfo";
			this->Load += gcnew System::EventHandler(this, &DebugInfo::DebugInfo_Load);
			this->ResumeLayout(false);

		}
#pragma endregion
	private: System::Void DebugInfo_Load(System::Object^  sender, System::EventArgs^  e) {
	}
	};
}
