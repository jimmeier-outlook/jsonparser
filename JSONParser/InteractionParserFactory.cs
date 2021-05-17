using JsonParser.InteractionParsers;

namespace JsonParser
{
    public class InteractionParserFactory
    {
        public IInteractionParser GetInteractionParser(InteractionType interactionType)
        {
            switch (interactionType)
            {
                case InteractionType.Null:
                    return new BlankInteractionParser();
                case InteractionType.TextBoxInteraction:
                    return new TextBoxInteractionParser();
                case InteractionType.barChartInteraction:
                    return new TextBoxInteractionParser();
                case InteractionType.numberLineInteraction:
                    return new TextBoxInteractionParser();
                case InteractionType.LineGraphingInteraction:
                    return new TextBoxInteractionParser();
                case InteractionType.lineGraphInteraction:
                    return new TextBoxInteractionParser();
                case InteractionType.FreeDrawInteraction:
                    return new TextBoxInteractionParser();
                case InteractionType.freeDrawInteraction:
                    return new TextBoxInteractionParser();
                case InteractionType.AudioRecInteraction:
                    return new AudioRecInteractionParser();
                case InteractionType.choiceInteraction:
                    return new ChoiceInteractionParser();
                case InteractionType.dragDropInteraction:
                    return new DragDropInteractionParser();
                case InteractionType.ImagemapInteraction:
                    return new ImageMapInteractionParser();
                case InteractionType.inlineChoiceInteraction:
                    return new DropDownInteractionParser();
                case InteractionType.SelectTextInteraction:
                    return new SelectTextInteractionParser();
                case InteractionType.BlankInteraction:
                    return new BlankInteractionParser();
                case InteractionType.textEntryInteraction:
                    return new FillInBlankInteractionParser();
               case InteractionType.LineMatchingInteraction2:
                    return new LineMatchingInteractionParser();
                case InteractionType.lineMatchInteraction:
                    return new LineMatchingInteractionParser();
                case InteractionType.matchInteraction:
                    return new RowColumnInteractionParser();
                case InteractionType.custominteraction:
                    return new TextBoxInteractionParser();
                case InteractionType.customInteraction:
                    return new TextBoxInteractionParser();
                case InteractionType.simulationitem:
                    return new TextBoxInteractionParser();
                case InteractionType.simulationinteraction:
                    return new TextBoxInteractionParser();
                case InteractionType.voicerecordinginteraction:
                    return new AudioRecInteractionParser();
                case InteractionType.gapmatchinteraction:
                    return new DragDropInteractionParser();
                case InteractionType.graphicgapmatchinteraction:
                    return new DragDropInteractionParser();
                case InteractionType.hotspotinteraction:
                    return new ImageMapInteractionParser();
                case InteractionType.hotspotInteraction:
                    return new ImageMapInteractionParser();
                case InteractionType.hottextinteraction:
                    return new SelectTextInteractionParser();
                case InteractionType.extendedtextinteraction:
                    return new TextBoxInteractionParser();
                case InteractionType.equationeditorinteraction:
                    return new TextBoxInteractionParser();
                case InteractionType.extendedTextInteraction:
                    return new TextBoxInteractionParser();
                case InteractionType.graphingLegendInteraction:
                    return new TextBoxInteractionParser();
                case  InteractionType.mediainteraction:
                    return new BlankInteractionParser();
                case InteractionType.gapMatchInteraction:
                    return new DragDropInteractionParser();
                case InteractionType.graphicGapMatchInteraction:
                    return new DragDropInteractionParser();
                case InteractionType.hottextInteraction:
                    return new SelectTextInteractionParser();
                case InteractionType.dragAndDropInteraction:
                    return new DragDropInteractionParser();
                case InteractionType.graphingHybridInteraction:
                    return new TextBoxInteractionParser();

            }
            return null;
        }
    }
}